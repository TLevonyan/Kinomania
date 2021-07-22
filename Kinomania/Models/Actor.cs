using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kinomania.Models
{
    public class Actor
    {
        [Key]
        [Column("ActorId")]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhotoPath { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Birthday { get; set; }

        public IList<ActorFilm> ActorsFilms { get; set; }



        [NotMapped]
        public IFormFile PhotoFile { get; set; }

        [NotMapped]
        public IList<Film> Films { get; set; }

        [NotMapped]
        public List<int> SelectedFilmsValues { get; set; }
    }
}
