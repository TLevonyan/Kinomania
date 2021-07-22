using Kinomania.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinomania.Models.Repositories
{
    public interface IReviewRepository
    {
        IEnumerable<Review> GetReviewsByFilm(int filmId);
        Review AddNewReview(Review review);
        Review UpdateReview(Review review);
        void DeleteReviewById(int id);
        void DeleteReview(Review review);
    }
}
