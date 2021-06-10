using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinomania.Models.Repositories
{
    interface IActorRepository
    {
        IEnumerable<Actor> GetActorsByFilm(Film film);
        Actor GetActorById(int id);
        Actor AddNewActor(Actor actor, IList<Film> films);
        Actor UpdateActor(Actor actor);
    }
}
