using System;
using System.Collections.Generic;
using System.IO;
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

        private readonly string _imagesfolderpath = @"C:\Users\levon\Desktop\C#\C#projects\Asp\Kinomania\Kinomania\wwwroot\Images\";



        public async Task<Actor> AddNewActorAsync(Actor actor)
        {
            string imgext = Path.GetExtension(actor.PhotoFile.FileName);
            actor.PhotoPath = "Actors/" + actor.PhotoFile.FileName;

            actor.Films = new List<Film>();

            if (actor.SelectedFilmsValues != null)
            {
                for (int i = 0; i < actor.SelectedFilmsValues.Count; i++)
                {
                    Film film = dbContext.Films.Find(actor.SelectedFilmsValues[i]);
                    actor.Films.Add(film);
                }
            }

            var imgpath = Path.Combine(_imagesfolderpath, actor.PhotoPath);
            var stream = new FileStream(imgpath, FileMode.Create);
            await actor.PhotoFile.CopyToAsync(stream);


            if (actor.Films != null)
            {
                for (int i = 0; i < actor.Films.Count; i++)
                {
                    ActorFilm actorFilm = new ActorFilm { Actor = actor, Film = actor.Films[i] };
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

        public async Task<Actor> GetActorByIdAsync(int id)
        {
            Actor actor = await dbContext.Actors
                .Include(a => a.ActorsFilms)
                .ThenInclude(x => x.Film)
                .FirstOrDefaultAsync(m => m.Id == id);

            return actor;
        }

        public IEnumerable<Actor> GetActorsByFilm(Film film)
        {
            return dbContext.Actors.Where(f => dbContext.ActorsFilms.Where(x => x.FilmId == film.Id).First(z => z.ActorId == f.Id) != null).ToList();
        }

        public void DeleteActor(Actor actor)
        {
            dbContext.ActorsFilms.RemoveRange(dbContext.ActorsFilms.Where(x => x.ActorId == actor.Id));

            File.Delete(_imagesfolderpath + actor.PhotoPath);
            dbContext.Actors.Remove(actor);
            dbContext.SaveChanges();
        }

        public void DeleteActorById(int id)
        {
            DeleteActor(dbContext.Actors.Find(id));
        }
    }
}
