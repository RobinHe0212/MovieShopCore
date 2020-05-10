using AutoMapper;
using MovieShop.Core.ApiModels.Request;
using MovieShop.Core.ApiModels.Response;
using MovieShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
            CreateMap<FavouriteRequestModel, Favourite>();
            CreateMap<IEnumerable<Favourite>, FavouriteResponseModel>()
                .ForMember(f => f.FavMovies, opt => opt.MapFrom(src => GetFavsForUser(src)))
                .ForMember(f=>f.UserId,opt=>opt.MapFrom(src=>src.FirstOrDefault().UserId));
            CreateMap<ReviewRequestModel, Review>();
            CreateMap<IEnumerable<Review>, ReviewResponseModel>()
                .ForMember(f => f.Movies, opt => opt.MapFrom(src => GetReviews(src)))
                .ForMember(f=>f.UserId,opt=>opt.MapFrom(src=>src.FirstOrDefault().UserId));

        }

        private List<ReviewMovieResponse> GetReviews(IEnumerable<Review> reviews)
        {
            List<ReviewMovieResponse> list = new List<ReviewMovieResponse>();
            foreach (var review in reviews)
            {
                list.Add(new ReviewMovieResponse
                {
                    MovieId = review.MovieId,
                    Name = review.Movie.Title,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText,
                    UserId = review.UserId

                });
            }
            return list;
        }

        private List<MovieResponseModel> GetFavsForUser(IEnumerable<Favourite> favs)
        {
            var favMovies = new List<MovieResponseModel>();
            foreach (var fav in favs)
            {
                favMovies.Add(new MovieResponseModel
                {
                    Id = fav.Id,
                    PosterUrl = fav.Movie.PosterUrl,
                    ReleaseDate = fav.Movie.ReleaseDate.Value,
                    Title = fav.Movie.Title
                });
            }
            return favMovies;
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
