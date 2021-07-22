using Kinomania.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinomania.Models.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext dbContext;
        public ReviewRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Review AddNewReview(Review review)
        {
            dbContext.Reviews.Add(review);
            dbContext.SaveChanges();
            return review;
        }

        public void DeleteReview(Review review)
        {
            dbContext.Reviews.Remove(review);
            dbContext.SaveChanges();
        }

        public void DeleteReviewById(int id)
        {
            dbContext.Reviews.Remove(dbContext.Reviews.Find(id));
            dbContext.SaveChanges();
        }

        public IEnumerable<Review> GetReviewsByFilm(int filmId)
        {
            return dbContext.Reviews.Where(x => x.FilmId == filmId).ToList();
        }

        public Review UpdateReview(Review review)
        {
            dbContext.Reviews.Update(review);
            dbContext.SaveChanges();
            return review;
        }
    }
}
