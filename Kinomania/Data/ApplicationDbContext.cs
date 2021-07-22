using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Kinomania.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kinomania.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        public IList<UserGenre> UsersGenres { get; set; }



        [NotMapped]
        public IList<Genre> Genres { get; set; }

        [NotMapped]
        public List<int> SelectedGenresValues { get; set; }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorFilm> ActorsFilms { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<FilmGenre> FilmsGenres { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<UserGenre> UsersGenres { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Review> Reviews { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ActorFilm>().HasKey(x => new { x.ActorId, x.FilmId });
            builder.Entity<FilmGenre>().HasKey(x => new { x.FilmId, x.GenreId });
            builder.Entity<UserGenre>().HasKey(x => new { x.UserId, x.GenreId });

            builder.Entity<Genre>()
                .Property(x => x.Name)
                .HasConversion<string>();

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            builder.Entity<ApplicationUser>().ToTable("Users").Property(p => p.Id).HasColumnName("UserId");
            builder.Entity<IdentityRole>().ToTable("UserRoles");

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
