using MovieShop.Core.ApiModels.Request;
using MovieShop.Core.ApiModels.Response;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptoService _cryptoService;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository,ICryptoService cryptoService, IPurchaseRepository purchaseRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _cryptoService = cryptoService;
            _purchaseRepository = purchaseRepository;
            _roleRepository = roleRepository;
        }
        public async Task<UserRegisterResponseModel> CreateUser(UserRegisterRequestModel reqeust)
        {
            var dbUser = await _userRepository.GetUserByEmail(reqeust.Email);
            if (dbUser != null)
            {
                throw new Exception("Email already exists");
            }
            var salt = _cryptoService.GetSalt();
            var hashingpsw = _cryptoService.HashingPassword(reqeust.Password, salt);
            var user = new User
            {
                Email = reqeust.Email,
                FirstName = reqeust.FirstName,
                LastName = reqeust.LastName,
                Salt = salt,
                HashedPassword = hashingpsw
            };
            var createdUser = await _userRepository.AddAsync(user);
            var response = new UserRegisterResponseModel
            {
                Id = createdUser.Id,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email
            };
            return response;
        }

        public async Task<PurchaseResponseModel> GetAllPurchasedMoviesByUser(int id)
        {
            var movies = await _purchaseRepository.ListAllWithIncludesAsync(
                p=>p.UserId==id,
                p=>p.Movie
                );
            return GetAllPurchasedMovies(movies,id);
        }

        //get role for user
        public async Task<UserRoleRespnseModel> GetRolesForUser(int id)
        {
            var roles = await _roleRepository.ListAllWithIncludesAsync(r => r.UserId == id, r => r.Role);
            return GetAllRolesForUser(roles, id);
        }

        private UserRoleRespnseModel GetAllRolesForUser(IEnumerable<UserRole> users, int id)
        {
            var roles = new List<RoleResponseModel>();
            foreach (var user in users)
            {
                roles.Add(new RoleResponseModel
                {
                    RoleId = user.RoleId,
                    Name = user.Role.Name
                });
            }
            return new UserRoleRespnseModel { UserId = id, Roles = roles };
        }

        private PurchaseResponseModel GetAllPurchasedMovies(IEnumerable<Purchase> purchases,int userId)
        {
            var movies = new List<PurchasedMovieResponseModel>();
            foreach (var pur in purchases)
            {
                movies.Add(new PurchasedMovieResponseModel {
                     Id=pur.MovieId,
                     PosterUrl =pur.Movie.PosterUrl,
                     PurchasedDateTime=pur.PurchaseDateTime,
                     Title = pur.Movie.Title
                });
            }
            return new PurchaseResponseModel { UserId = userId, PurchasedMovies = movies };
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public async Task<User> ValidateUser(string email, string password)
        {
            var dbUser = await GetUserByEmail(email);
            if (dbUser == null)
            {
                return null;
            }
            var hashingPassword = _cryptoService.HashingPassword(password, dbUser.Salt);
            if (hashingPassword == dbUser.HashedPassword)
            {
                return dbUser;
            }
            else
            {
                throw new Exception("wrong password or email");
            }
        }
    }
}
