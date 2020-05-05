using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Infrastructure.Data
{
   public class MovieShopDbContext: DbContext
    {
        public MovieShopDbContext(DbContextOptions<MovieShopDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<MovieGenre> MovieGenres { get; set; }

        public DbSet<Cast> Casts { get; set; }

        public DbSet<MovieCast> MovieCasts { get; set; }

        public DbSet<Crew> Crew { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<MovieCrew> MovieCrews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Genre>(ConfigureGenre);
            modelBuilder.Entity<Movie>(ConfigureMovie);
            modelBuilder.Entity<MovieGenre>(ConfiguareMovieGenres);
            modelBuilder.Entity<Cast>(ConfigureCast);
            modelBuilder.Entity<MovieCast>(ConfigureMovieCasts);
            modelBuilder.Entity<User>(ConfigureUser);
            modelBuilder.Entity<Role>(ConfigureRole);
            modelBuilder.Entity<UserRole>(ConfigureUserRoles);
            modelBuilder.Entity<MovieCrew>(ConfigureMovieCrew);
            modelBuilder.Entity<Crew>(ConfigureCrew);
            modelBuilder.Entity<Purchase>(ConfigurePurchase);

        }

        private void ConfigurePurchase(EntityTypeBuilder<Purchase> builder)
        {
            builder.ToTable("Purchase");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.PurchaseNumber).ValueGeneratedOnAdd();
            builder.HasIndex(p => new { p.UserId, p.MovieId }).IsUnique();
        }

        private void ConfigureCrew(EntityTypeBuilder<Crew> builder)
        {
            builder.ToTable("Crew");

            builder.HasKey(c => c.Id);

            builder.HasIndex(c => c.Name);

            builder.Property(c => c.Name).HasMaxLength(128);

            builder.Property(c => c.ProfilePath).HasMaxLength(2084);
        }

        private void ConfigureMovieCrew(EntityTypeBuilder<MovieCrew> builder)
        {
            builder.ToTable("MovieCrew");

            builder.Property(c => c.Job).HasMaxLength(128);

            builder.Property(c => c.Department).HasMaxLength(128);

            builder.HasKey(mc => new { mc.MovieId, mc.CrewId, mc.Department, mc.Job });

            builder.HasOne(mc => mc.Movie).WithMany(mc => mc.CrewsOfMovie).HasForeignKey(mc => mc.MovieId);

            builder.HasOne(mc => mc.Crew).WithMany(mc => mc.MoviesOfCrew).HasForeignKey(mc => mc.CrewId);
        }

        private void ConfigureUserRoles(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole");

            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.HasOne(ur => ur.Role).WithMany(ur => ur.UsersOfRole).HasForeignKey(ur => ur.RoleId);

            builder.HasOne(ur => ur.User).WithMany(ur => ur.RolesOfUser).HasForeignKey(ur => ur.UserId);
        }

        private void ConfigureRole(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name).HasMaxLength(20);
        }

        private void ConfigureUser(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.Email).HasMaxLength(256);

            builder.Property(u => u.FirstName).HasMaxLength(128);

            builder.Property(u => u.LastName).HasMaxLength(128);

            builder.Property(u => u.HashedPassword).HasMaxLength(1024);

            builder.Property(u => u.PhoneNumber).HasMaxLength(16);

            builder.Property(u => u.Salt).HasMaxLength(1024);

            builder.Property(u => u.IsLocked).HasDefaultValue(false);
        }

        private void ConfigureMovieCasts(EntityTypeBuilder<MovieCast> builder)
        {
            builder.ToTable("MovieCast");
            builder.HasKey(mc => new { mc.CastId, mc.MovieId, mc.Character });
            builder.HasOne(mc => mc.Movie).WithMany(mc => mc.CastsOfMovie).HasForeignKey(mc => mc.MovieId);
            builder.HasOne(mc => mc.Cast).WithMany(mc => mc.MoviesOfCast).HasForeignKey(mc => mc.CastId);
        }

        private void ConfigureCast(EntityTypeBuilder<Cast> builder)
        {
            builder.ToTable("Cast");
            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.Name);
            builder.Property(c => c.Name).HasMaxLength(128);
            builder.Property(c => c.ProfilePath).HasMaxLength(2084);
        }

        private void ConfiguareMovieGenres(EntityTypeBuilder<MovieGenre> builder)
        {
            builder.ToTable("MovieGenre");
            builder.HasKey(mg => new { mg.GenreId, mg.MovieId });
            builder.HasOne(mg => mg.Movie).WithMany(g => g.GenresOfMovie).HasForeignKey(mg => mg.MovieId);
            builder.HasOne(mg => mg.Genre).WithMany(m => m.MoviesOfGenre).HasForeignKey(mg => mg.GenreId);
        }

        private void ConfigureMovie(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movie");

            builder.HasKey(m => m.Id);

            builder.HasIndex(m => m.Title);

            builder.Property(m => m.Title).IsRequired().HasMaxLength(256);

            builder.Property(m => m.Overview).HasMaxLength(4096);

            builder.Property(m => m.Tagline).HasMaxLength(512);

            builder.Property(m => m.ImdbUrl).HasMaxLength(2084);

            builder.Property(m => m.TmdbUrl).HasMaxLength(2084);

            builder.Property(m => m.PosterUrl).HasMaxLength(2084);

            builder.Property(m => m.BackdropUrl).HasMaxLength(2084);

            builder.Property(m => m.OriginalLanguage).HasMaxLength(64);

            builder.Property(m => m.Price).HasColumnType("decimal(5, 2)").HasDefaultValue(9.9m);

            builder.Property(m => m.CreatedDate).HasDefaultValueSql("getdate()");

            builder.Property(m => m.Budget).HasColumnType("decimal(18,2)");

            builder.Property(m => m.Revenue).HasColumnType("decimal(18,2)");

            builder.Ignore(m => m.Rating);
        }

        private void ConfigureGenre(EntityTypeBuilder<Genre> builder)
        {
            builder.ToTable("Genre");
            builder.HasKey(g => g.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(64);
        }
    }
}
