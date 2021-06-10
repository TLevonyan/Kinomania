using Kinomania.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinomania.Models.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext dbContext;
        public RatingRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Rating AddNewRating(Rating rating)
        {
            dbContext.Ratings.Add(rating);
            dbContext.SaveChanges();
            return rating;
        }

        public Rating GetRatingByFilmAndUser(int filmId, string userId)
        {
            return dbContext.Ratings.Where(x => x.FilmId == filmId).First(v => v.UserId == userId);
        }

        public IEnumerable<Rating> GetRatingsByFilm(int filmId)
        {
            return dbContext.Ratings.Where(x => x.FilmId == filmId).ToList();
        }

        public Rating UpdateRating(Rating rating)
        {
            dbContext.Ratings.Update(rating);
            dbContext.SaveChanges();
            return rating;
        }
    }
}
