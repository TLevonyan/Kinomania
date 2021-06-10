using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinomania.Models.Repositories
{
    interface IFilmRepository
    {
        IEnumerable<Film> AllFilms();
        Film GetFilmById(int id);
        Film UpdateFilm(Film film);
        IEnumerable<Film> GetFilmsByGenre(Genre genre);
        IEnumerable<Film> GetFilmsByActor(Actor actor);
        Film AddNewFilm(Film film, IList<Actor> actors, IList<Genre> genres);

    }
}
