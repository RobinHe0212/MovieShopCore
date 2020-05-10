using MovieShop.Core.ApiModels.Request;
using MovieShop.Core.ApiModels.Response;
using MovieShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Core.ServiceInterfaces
{
   public interface IUserService
    {
       Task<User> ValidateUser(string email, string password);
       Task<UserRegisterResponseModel> CreateUser(UserRegisterRequestModel reqeust);
        Task<User> GetUserByEmail(string email);

        Task<PurchaseResponseModel> GetAllPurchasedMoviesByUser(int id);
        Task<UserRoleRespnseModel> GetRolesForUser(int id);

        Task<FavouriteResponseModel> GetAllFavouriteForUser(int id);

        Task RemoveFavorite(FavouriteRequestModel fav);

        Task AddFavourite(FavouriteRequestModel fav);

        Task<ReviewResponseModel> GetAllReviewsByUser(int userId);

        Task DeleteReview(int userId, int movieId);

        Task UpdateReview(ReviewRequestModel review);
        Task AddReview(ReviewRequestModel review);
    }
}
