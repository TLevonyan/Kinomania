using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kinomania.Data;

namespace Kinomania.Models.Repositories
{
    public interface IRatingRepository
    {
        Rating GetRatingByFilmAndUser(int filmId, string userId);
        IEnumerable<Rating> GetRatingsByFilm(int filmId);
        Rating AddNewRating(Rating rating);
        Rating UpdateRating(Rating rating);
    }
}
