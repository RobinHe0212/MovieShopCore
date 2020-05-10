using AutoMapper;
using MovieShop.Core.ApiModels.Request;
using MovieShop.Core.ApiModels.Response;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IAsyncRepository<Favourite> _favRepository;
        private readonly IAsyncRepository<Review> _reviewRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,IMapper mapper,IAsyncRepository<Review> reviewRepository,IAsyncRepository<Favourite> favRepository,ICryptoService cryptoService, IPurchaseRepository purchaseRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _cryptoService = cryptoService;
            _purchaseRepository = purchaseRepository;
            _roleRepository = roleRepository;
            _favRepository = favRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
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

        //add Fav
        public async Task AddFavourite(FavouriteRequestModel fav)
        {
            //check if fav already exits
            if(await FavExist(fav.UserId, fav.MovieId))
            {
                throw new Exception("Movie already exists");
            }
            var favourite = _mapper.Map<Favourite>(fav);
            await _favRepository.AddAsync(favourite);
        }

        public async Task<bool> FavExist(int userId, int movieId)
        {
            return await _favRepository.GetExistsAsync(f => f.UserId == userId && f.MovieId == movieId);
        }

        public async Task RemoveFavorite(FavouriteRequestModel fav)
        {
            var dbFav = await _favRepository.ListAsync(f => f.UserId == fav.UserId && f.MovieId == fav.MovieId);
            await _favRepository.DeleteAsync(dbFav.First());
        }

        public async Task<FavouriteResponseModel> GetAllFavouriteForUser(int id)
        {
            var favouriteMovies = await _favRepository.ListAllWithIncludesAsync(
                 f=>f.UserId==id,
                 p=>p.Movie
                );
            return _mapper.Map<FavouriteResponseModel>(favouriteMovies);
        }

        // review 
        public async Task AddReview(ReviewRequestModel review)
        {
            var reviewReq = _mapper.Map<Review>(review);
            await _reviewRepository.AddAsync(reviewReq);
        }

        public async Task UpdateReview(ReviewRequestModel review)
        {
            var reviewReq = _mapper.Map<Review>(review);
            await _reviewRepository.UpdateAsync(reviewReq);
        }

        public async Task DeleteReview(int userId,int movieId)
        {
            var review = await _reviewRepository.ListAsync(r => r.UserId == userId && r.MovieId == movieId);
            await _reviewRepository.DeleteAsync(review.First());
        }

        public async Task<ReviewResponseModel> GetAllReviewsByUser(int userId)
        {
            var reviews = await _reviewRepository.ListAllWithIncludesAsync(r => r.UserId == userId, r => r.Movie);
            return _mapper.Map<ReviewResponseModel>(reviews);
        }
    }
}
