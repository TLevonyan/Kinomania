using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinomania.Models
{
    public enum GenreTypes
    {
        Action,
        Adventure,
        Animation,
        Biographical,
        Comedy,
        Crime,
        Drama,
        Fantasy,
        Historical,
        Horror,
        Musical,
        Mystery,
        Romance,
        Sport,
        Thriller,
        War,
        Western
    }
    public class Genre
    {

        public int Id { get; set; }
        public GenreTypes Name { get; set; }

        public IList<FilmGenre> FilmsGenres { get; set; }
        public IList<UserGenre> UsersGenres { get; set; }

    }
}
