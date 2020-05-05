using AutoMapper;
using MovieShop.Core.ApiModels.Response;
using MovieShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Core.MappingProfiles
{
   public class MoviesMappingProfile:Profile
    {
        public MoviesMappingProfile()
        {
            CreateMap<Movie, MovieResponseModel>();
            CreateMap<Cast, CastDetailsResponseModel>()
                .ForMember(c => c.Movies, opt => opt.MapFrom(src => GetMoviesForCast(src.MoviesOfCast)));
            CreateMap<Movie, MovieDetailsResponseModel>()
                .ForMember(m => m.Casts, opt => opt.MapFrom(src => GetCasts(src.CastsOfMovie)))
                .ForMember(m => m.Genres, opt => opt.MapFrom(src=>GetGenres(src.GenresOfMovie)));
        }

        private List<GenreResponseModel> GetGenres(IEnumerable<MovieGenre> genresOfMovie)
        {
            var movieGenres = new List<GenreResponseModel>();
            foreach (var genre in genresOfMovie)
            {
                movieGenres.Add(new GenreResponseModel
                {
                    Id = genre.GenreId,
                    Name = genre.Genre.Name
                });
            }
            return movieGenres;
        }

        private List<CastReponseModel> GetCasts(IEnumerable<MovieCast> castsOfMovie)
        {
            var movieCasts = new List<CastReponseModel>();
            foreach (var cast in castsOfMovie)
            {
                movieCasts.Add(new CastReponseModel
                {
                    Id = cast.CastId,
                    Name=cast.Cast.Name,
                    Gender=cast.Cast.Gender,
                    ProfilePath=cast.Cast.ProfilePath,
                    TmdbUrl=cast.Cast.TmdbUrl,
                    Character = cast.Character
                    
                }); 
            }
            return movieCasts;
        }

        private List<MovieResponseModel> GetMoviesForCast(IEnumerable<MovieCast> movieCasts)
        {
            var castMovies = new List<MovieResponseModel>();
            foreach (var movie in movieCasts)
            {
                castMovies.Add(new MovieResponseModel
                {
                    Id = movie.MovieId,
                    PosterUrl = movie.Movie.PosterUrl,
                    Title = movie.Movie.Title
                });
            }
            return castMovies;
        }
    }
}
