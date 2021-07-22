using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinomania.Models.Repositories
{
    public interface IFilmRepository : IRatingRepository,IReviewRepository
    {
        List<Film> AllFilms();
        Task<Film> GetFilmByIdAsync(int id);
        Film UpdateFilm(Film film);
        IEnumerable<Film> GetFilmsByGenre(Genre genre);
        IEnumerable<Film> GetFilmsByActor(Actor actor);
        Task<Film> AddNewFilmAsync(Film film);
        void DeleteFilm(Film film);
        void DeleteFilmById(int id);

    }
}
