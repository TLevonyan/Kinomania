using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kinomania.Models
{
    public class Film
    {
        [Key]
        [Column("FilmId")]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PosterPath { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Producer { get; set; }

        public decimal Budget { get; set; }

        public IList<ActorFilm> ActorsFilms { get; set; }

        public IList<FilmGenre> FilmsGenres { get; set; }

        public IList<Review> Reviews { get; set; }

        public IList<Rating> Ratings { get; set; }

        
        [NotMapped]
        public IFormFile PosterFile { get; set; }

        public IList<Actor> Actors { get; set; }

        public IList<Genre> Genres { get; set; }
    }
}
