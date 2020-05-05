using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MovieShop.Infrastructure.Repositories
{
    public class MovieRepository : EfRepository<Movie>,IMovieRepository
    {
        public MovieRepository(MovieShopDbContext dbContext):base(dbContext)
        {

        }

        public async Task<IEnumerable<Movie>> GetMoviesByCast(int castId)
        {
            var movies = await _dbContext.MovieCasts.Where(mc => mc.CastId == castId).Include(c => c.Movie).Select(m => m.Movie).ToListAsync();
            return movies;
        }

        public async Task<IEnumerable<Movie>> GetMoviesByGenre(int genreId)
        {
            var movies = await _dbContext.MovieGenres.Where(mg => mg.GenreId == genreId).Include(g => g.Movie).Select(m => m.Movie).ToListAsync();
            return movies;
        }



        public async Task<IEnumerable<Movie>> GetTopRevenueMovie()
        {
            var movie = await _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(20).ToListAsync();
            return movie;
        }

        public override async Task<Movie> GetByIdAsync(int id)

        {

            var movie = await _dbContext.Movies

                                        .Include(m => m.CastsOfMovie).ThenInclude(m => m.Cast).Include(m => m.GenresOfMovie)

                                        .ThenInclude(m => m.Genre)

                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return null;



            return movie;

        }
    }
}
