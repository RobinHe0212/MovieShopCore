using MovieShop.Core.ApiModels.Response;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MovieShop.Core.Helpers;
using System.Linq.Expressions;
using System.Linq;

namespace MovieShop.Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        public MovieService(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Movie>> GetTopGrossingMovies()
        {
            var movies = await _movieRepository.GetTopRevenueMovie();
            return movies;
        }

        public async Task<MovieDetailsResponseModel> GetMovieById(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            var response = _mapper.Map<MovieDetailsResponseModel>(movie);
            return response;
        }

        public async Task<IEnumerable<Movie>> GetMovieByGenre(int genreId)
        {
            var movies = await _movieRepository.GetMoviesByGenre(genreId);
            return movies;
        }

        public async Task<IEnumerable<Movie>> GetMoviesByCast(int castId)
        {
            var movies = await _movieRepository.GetMoviesByCast(castId);
            return movies;
        }

        public async Task<PageResultSet<MovieResponseModel>> GetMoviesByPagination(int pageSize = 20, int page = 0, string title = "")
        {
            Expression<Func<Movie, bool>> filterExpression = null;
            if (!string.IsNullOrEmpty(title))
            {
                filterExpression = movie => title != null && movie.Title.Contains(title);
            }
            var pagedMovies = await _movieRepository.GetPagedData(page, pageSize, movie => movie.OrderBy(m => m.Title), filterExpression);
            var pagesResponseModel = new List<MovieResponseModel>();
            foreach (var movie in pagedMovies)
            {
                pagesResponseModel.Add(new MovieResponseModel {
                   Id = movie.Id,
                   PosterUrl = movie.PosterUrl,
                   ReleaseDate = movie.ReleaseDate.Value,
                   Title = title
                });
            }
            return new PageResultSet<MovieResponseModel>(pagesResponseModel, page, pageSize, pagesResponseModel.Count());
        }
    }
}
