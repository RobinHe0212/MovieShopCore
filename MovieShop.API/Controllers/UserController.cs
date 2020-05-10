using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.ApiModels.Request;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        [Route("{id:int}/purchases")]
        public async Task<IActionResult> GetMoviesPurchasedByUser(int id)
        {
            var userMovies = await _userService.GetAllPurchasedMoviesByUser(id);
            return Ok(userMovies);
        }

        [HttpPost("favourite")]
        public async Task<IActionResult> AddFavourite([FromBody] FavouriteRequestModel favRequestModel)
        {
            await _userService.AddFavourite(favRequestModel);
            return Ok();
        }

        [HttpGet("{id:int}/favourites")]
        public async Task<IActionResult> GetUserAllFavMovies(int id)
        {
            var userMovies = await _userService.GetAllFavouriteForUser(id);
            return Ok(userMovies);
        }

        [HttpPost("unFavourite")]
        public async Task<IActionResult> DeleteFavMovie([FromBody] FavouriteRequestModel favRequestModel)
        {
            await _userService.RemoveFavorite(favRequestModel);
            return Ok();
        }

        [HttpPost("Review")]
        public async Task<IActionResult> AddReview([FromBody] ReviewRequestModel reviewReqModel)
        {

            await _userService.AddReview(reviewReqModel);
            return Ok();
        }

        [HttpPut("Review")]
        public async Task<IActionResult> UpdateReview([FromBody] ReviewRequestModel reviewReqModel)
        {
            await _userService.UpdateReview(reviewReqModel);
            return Ok();
        }

        [HttpDelete("{user:int}/movie/{movieId:int}")]
        public async Task<IActionResult> DeleteReview(int user,int movieId)
        {
            await _userService.DeleteReview(user, movieId);
            return Ok();
        }

        [HttpGet("{userId:int}/reviews")]
        public async Task<IActionResult> GetUserReviewMoviesAsync(int userId)
        {
            var userMovies = await _userService.GetAllReviewsByUser(userId);
            return Ok(userMovies);
        }
    }
}