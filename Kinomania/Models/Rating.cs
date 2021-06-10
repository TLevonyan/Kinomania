using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Kinomania.Data;

namespace Kinomania.Models
{
    public class Rating
    {
        public int Id { get; set; }

        [Required]
        [Range(0, 10)]
        public int Rate { get; set; }

        [ForeignKey("Film")]
        public int FilmId { get; set; }
        public Film Film { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
