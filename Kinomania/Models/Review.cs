using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Kinomania.Data;
using System.Threading.Tasks;

namespace Kinomania.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1, MinimumLength = 300)]
        public string Body { get; set; }

        public DateTime Datetime { get; set; } = DateTime.Now;

        [ForeignKey("Film")]
        public int FilmId { get; set; }
        public Film Film { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
