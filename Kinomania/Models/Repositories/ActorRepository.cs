using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kinomania.Data;
using Microsoft.EntityFrameworkCore;

namespace Kinomania.Models.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly ApplicationDbContext dbContext;
        public ActorRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private readonly string _imagesfolderpath = @"C:\Users\levon\Desktop\C#\C#projects\Asp\Kinomania\Kinomania\wwwroot\Images\Actors\";

        public string ImagesfolderPath => _imagesfolderpath;


        public Actor AddNewActor(Actor actor, IList<Film> films)
        {
            if(films != null)
            {
                for (int i = 0; i < films.Count; i++)
                {
                    ActorFilm actorFilm = new ActorFilm { Actor = actor, Film = films[i] };
                    dbContext.ActorsFilms.Add(actorFilm);
                }
            }
            dbContext.Actors.Add(actor);

            dbContext.SaveChanges();

            return actor;
        }

        public Actor UpdateActor(Actor actor)
        {
            dbContext.Actors.Update(actor);
            dbContext.SaveChanges();
            return actor;
        }

        public Actor GetActorById(int id)
        {
            return dbContext.Actors.Find(id);
        }

        public IEnumerable<Actor> GetActorsByFilm(Film film)
        {
            return dbContext.Actors.Where(f => dbContext.ActorsFilms.Where(x => x.FilmId == film.Id).First(z => z.ActorId == f.Id) != null).ToList();
        }
    }
}
