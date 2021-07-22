using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinomania.Models.Repositories
{
    public interface IActorRepository
    {
        IEnumerable<Actor> GetActorsByFilm(Film film);
        Task<Actor> GetActorByIdAsync(int id);
        Task<Actor> AddNewActorAsync(Actor actor);
        Actor UpdateActor(Actor actor);
        void DeleteActor(Actor actor);
        void DeleteActorById(int id);
    }
}
