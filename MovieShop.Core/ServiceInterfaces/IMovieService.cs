using MovieShop.Core.ApiModels.Response;
using MovieShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Core.ServiceInterfaces
{
   public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetTopGrossingMovies();
        Task<MovieDetailsResponseModel> GetMovieById(int id);

        Task<IEnumerable<Movie>> GetMoviesByCast(int castId);

        Task<IEnumerable<Movie>> GetMovieByGenre(int genreId);
    }
}
