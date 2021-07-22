using Kinomania.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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

        private readonly string _imagesfolderpath = @"C:\Users\levon\Desktop\C#\C#projects\Asp\Kinomania\Kinomania\wwwroot\Images\";

        public async Task<Film> AddNewFilmAsync(Film film)
        {
            film.PosterPath = "Films/" + film.PosterFile.FileName;

            film.Actors = new List<Actor>();
            film.Genres = new List<Genre>();

            if (film.SelectedActorsValues != null)
            {
                for (int i = 0; i < film.SelectedActorsValues.Count; i++)
                {
                    Actor actor = dbContext.Actors.Find(film.SelectedActorsValues[i]);
                    film.Actors.Add(actor);
                }
            }

            if (film.SelectedGenresValues != null)
            {
                for (int i = 0; i < film.SelectedGenresValues.Count; i++)
                {
                    Genre genre = dbContext.Genres.Find(film.SelectedGenresValues[i]);
                    film.Genres.Add(genre);
                }
            }

            var imgpath = Path.Combine(_imagesfolderpath, film.PosterPath);
            var stream = new FileStream(imgpath, FileMode.Create);
            await film.PosterFile.CopyToAsync(stream);


            int n = 0;
            while (n < film.Actors?.Count || n < film.Genres?.Count)
            {
                if (n < film.Actors.Count)
                {
                    ActorFilm actorfilm = new ActorFilm { Film = film, Actor = film.Actors[n] };
                    dbContext.ActorsFilms.Add(actorfilm);
                }
                if (n < film.Genres.Count)
                {
                    FilmGenre filmgenre = new FilmGenre { Film = film, Genre = film.Genres[n] };
                    dbContext.FilmsGenres.Add(filmgenre);
                }
                n++;
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

        public List<Film> AllFilms()
        {
            return dbContext.Films.ToList();
        }

        public async Task<Film> GetFilmByIdAsync(int id)
        {
            Film film = await dbContext.Films
                .Include(a => a.FilmsGenres)
                .ThenInclude(x => x.Genre)
                .Include(b => b.ActorsFilms)
                .ThenInclude(y => y.Actor)
                .Include(c => c.Reviews)
                .ThenInclude(z=>z.User)
                .Include(d => d.Ratings)
                .FirstOrDefaultAsync(m => m.Id == id);
            return film;
        }

        public IEnumerable<Film> GetFilmsByActor(Actor actor)
        {
            return dbContext.Films.Where(f => dbContext.ActorsFilms.Where(x => x.ActorId == actor.Id).First(z => z.FilmId == f.Id) != null).ToList();
        }

        public IEnumerable<Film> GetFilmsByGenre(Genre genre)
        {
            return dbContext.Films.Where(f => dbContext.FilmsGenres.Where(x => x.GenreId == genre.Id).First(z => z.FilmId == f.Id) != null).ToList();
        }

        public void DeleteFilm(Film film)
        {
            dbContext.Reviews.RemoveRange(dbContext.Reviews.Where(x => x.FilmId == film.Id));
            dbContext.Ratings.RemoveRange(dbContext.Ratings.Where(x => x.FilmId == film.Id));
            dbContext.FilmsGenres.RemoveRange(dbContext.FilmsGenres.Where(x => x.FilmId == film.Id));
            dbContext.ActorsFilms.RemoveRange(dbContext.ActorsFilms.Where(x => x.FilmId == film.Id));

            File.Delete(_imagesfolderpath + film.PosterPath);
            dbContext.Films.Remove(film);
            dbContext.SaveChanges();
        }

        public void DeleteFilmById(int id)
        {
            DeleteFilm(dbContext.Films.Find(id));
        }

        public Rating GetRatingByFilmAndUser(int filmId, string userId)
        {
            RatingRepository ratingRepository = new RatingRepository(dbContext);
            return ratingRepository.GetRatingByFilmAndUser(filmId,userId);
        }

        public IEnumerable<Rating> GetRatingsByFilm(int filmId)
        {
            RatingRepository ratingRepository = new RatingRepository(dbContext);
            return ratingRepository.GetRatingsByFilm(filmId);
        }

        public Rating AddNewRating(Rating rating)
        {
            RatingRepository ratingRepository = new RatingRepository(dbContext);
            return ratingRepository.AddNewRating(rating);
        }

        public Rating UpdateRating(Rating rating)
        {
            RatingRepository ratingRepository = new RatingRepository(dbContext);
            return ratingRepository.UpdateRating(rating);
        }

        public IEnumerable<Review> GetReviewsByFilm(int filmId)
        {
            ReviewRepository reviewRepository = new ReviewRepository(dbContext);
            return reviewRepository.GetReviewsByFilm(filmId);
        }

        public Review AddNewReview(Review review)
        {
            ReviewRepository reviewRepository = new ReviewRepository(dbContext);
            return reviewRepository.AddNewReview(review);
        }

        public Review UpdateReview(Review review)
        {
            ReviewRepository reviewRepository = new ReviewRepository(dbContext);
            return reviewRepository.UpdateReview(review);
        }

        public void DeleteReviewById(int id)
        {
            ReviewRepository reviewRepository = new ReviewRepository(dbContext);
            reviewRepository.DeleteReviewById(id);
        }

        public void DeleteReview(Review review)
        {
            ReviewRepository reviewRepository = new ReviewRepository(dbContext);
            reviewRepository.DeleteReview(review);
        }
    }
}
