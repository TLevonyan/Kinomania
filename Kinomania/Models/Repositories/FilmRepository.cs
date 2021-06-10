using Kinomania.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinomania.Models.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private readonly ApplicationDbContext dbContext;
        public FilmRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private readonly string _imagesfolderpath = @"C:\Users\levon\Desktop\C#\C#projects\Asp\Kinomania\Kinomania\wwwroot\Images\Films\";

        public string ImagesfolderPath => _imagesfolderpath;
        // TODO add exceptions
        public Film AddNewFilm(Film film, IList<Actor> actors, IList<Genre> genres)
        {
            int i = 0;
            while (i < actors?.Count || i < genres?.Count)
            {
                if (i < actors.Count)
                {
                    ActorFilm actorfilm = new ActorFilm { Film = film, Actor = actors[i] };
                    dbContext.ActorsFilms.Add(actorfilm);
                }
                if (i < genres.Count)
                {
                    FilmGenre filmgenre = new FilmGenre { Film = film, Genre = genres[i] };
                    dbContext.FilmsGenres.Add(filmgenre);
                }
                i++;
            }
            dbContext.Films.Add(film);
           
            dbContext.SaveChanges();

            return film;
        }
        public Film UpdateFilm(Film film)
        {
            dbContext.Films.Update(film);
            dbContext.SaveChanges();
            return film;
        }

        public IEnumerable<Film> AllFilms()
        {
            return dbContext.Films.ToList();
        }

        public Film GetFilmById(int id)
        {
            return dbContext.Films.Find(id);
        }

        public IEnumerable<Film> GetFilmsByActor(Actor actor)
        {
            return dbContext.Films.Where(f => dbContext.ActorsFilms.Where(x => x.ActorId == actor.Id).First(z => z.FilmId == f.Id) != null).ToList();
        }

        public IEnumerable<Film> GetFilmsByGenre(Genre genre)
        {
            return dbContext.Films.Where(f => dbContext.FilmsGenres.Where(x => x.GenreId == genre.Id).First(z => z.FilmId == f.Id) != null).ToList();
        }

    }
}
