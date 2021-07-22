using Kinomania.Models.Repositories;
using Kinomania.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Kinomania.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Kinomania.Controllers
{
    public class FilmController : Controller
    {
        private readonly IActorRepository actorRepository;
        private readonly IFilmRepository filmRepository;
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public FilmController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IActorRepository actorRepository, IFilmRepository filmRepository)
        {
            this._dbcontext = dbContext;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this.actorRepository = actorRepository;
            this.filmRepository = filmRepository;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            Random r = new Random();
            List<Film> allfilms = filmRepository.AllFilms().OrderBy(x => r.Next()).ToList();

            var user = await _userManager.GetUserAsync(User);
            if(user != null)
            {
                List<Genre> likedGenres = _dbcontext.UsersGenres.Where(x => x.UserId == user.Id)?.Select(a=>a.Genre).ToList();
                if(likedGenres != null)
                {
                    List<Film> likedGenresFilms = _dbcontext.FilmsGenres.Where(x => likedGenres.Any(z => z == x.Genre)).Select(a=>a.Film).Distinct().ToList();
                    likedGenresFilms = likedGenresFilms.OrderBy(x => r.Next()).Take(4).ToList();
                    ViewBag.RecommandatedFilms = likedGenresFilms;
                }
            }


            return View(allfilms);
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(string searchString)
        {
            List<Film> searchedfilms = filmRepository.AllFilms().Where(x => x.Name.Contains(searchString)).ToList();
            return View(searchedfilms);
        }



        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> FilmDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            Film film = await filmRepository.GetFilmByIdAsync((int)id);

            var user = await _userManager.GetUserAsync(User);

            try
            {
                film.Rating = film.Ratings.Select(x => x.Rate).Average();
            }
            catch
            {
                film.Rating = 0;
            }

            ViewBag.CurrentUserRating = film.Ratings.FirstOrDefault(x => x.User == user)?.Rate;

            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> ActorDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Actor actor = await actorRepository.GetActorByIdAsync((int)id);


            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }



        [Authorize(Policy = "RequireUserRole")]
        [HttpPost]
        public async Task<ActionResult> AddReview([FromForm]string Body,[FromForm] int FilmId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                var film = await filmRepository.GetFilmByIdAsync(FilmId);

                Review review = new Review
                {
                    Film = film,
                    User = user,
                    Body = Body,
                    Datetime = DateTime.Now
                };

                filmRepository.AddNewReview(review);


                return RedirectToAction("FilmDetails", film);
            }
            catch
            {
                return View();
            }
        }


        [Authorize(Policy = "RequireUserRole")]
        [HttpPost]
        public async Task<ActionResult> AddRating([FromForm] int Rate, [FromForm] int FilmId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                var film = await filmRepository.GetFilmByIdAsync(FilmId);

                Rating rating = new Rating
                {
                    Film = film,
                    User = user,
                    Rate = Rate
                };

                try
                {
                    var oldRating = filmRepository.GetRatingByFilmAndUser(FilmId, user.Id);
                    oldRating.Rate = rating.Rate;
                    filmRepository.UpdateRating(oldRating);
                }
                catch
                {
                    filmRepository.AddNewRating(rating);
                }


                return RedirectToAction("FilmDetails", film);
            }
            catch
            {
                return View();
            }
        }




        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpGet]
        public IActionResult AddFilm()
        {

            ViewBag.selectactors = new List<SelectListItem>();
            ViewBag.selectgenres = new List<SelectListItem>();

            foreach (Genre genre in _dbcontext.Genres.ToList())
            {
                ViewBag.selectgenres.Add(new SelectListItem { Text = genre.Name.ToString(), Value = genre.Id.ToString() });
            }

            foreach (Actor actor in _dbcontext.Actors.ToList())
            {
                ViewBag.selectactors.Add(new SelectListItem { Text = actor.FirstName + " " + actor.LastName, Value = actor.Id.ToString() });
            }

            return View("AddFilm"); 
        }


        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpPost]
        public async Task<IActionResult> AddFilm(Film film)
        {

            await filmRepository.AddNewFilmAsync(film);

            return RedirectToAction(nameof(Index));
        }



        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpGet]
        public IActionResult AddActor()
        {
            ViewBag.selectfilms = new List<SelectListItem>();

            foreach (Film film in _dbcontext.Films.ToList())
            {
                ViewBag.selectfilms.Add(new SelectListItem { Text = film.Name.ToString(), Value = film.Id.ToString() });
            }

            return View("AddActor");
        }



        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpPost]
        public async Task<IActionResult> AddActor(Actor actor)
        {
            await actorRepository.AddNewActorAsync(actor);

            return RedirectToAction(nameof(Index));
        }



        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpGet]
        public async Task<ActionResult> EditFilm(int id)
        {
            ViewBag.selectgenres = new List<SelectListItem>();

            foreach (Genre genre in _dbcontext.Genres.ToList())
            {
                ViewBag.selectgenres.Add(new SelectListItem { Text = genre.Name.ToString(), Value = genre.Id.ToString() });
            }


            Film film = await filmRepository.GetFilmByIdAsync(id);

            return View("EditFilm", film);
        }


        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpPost]
        public async Task<ActionResult> EditFilm(Film film)
        {
            try
            {
                Film updatefilm = await filmRepository.GetFilmByIdAsync(film.Id);

                if (film.SelectedGenresValues != null)
                {
                    updatefilm.Genres = new List<Genre>();

                    _dbcontext.FilmsGenres.RemoveRange(updatefilm.FilmsGenres);

                    updatefilm.FilmsGenres = new List<FilmGenre>();

                    for (int i = 0; i < film.SelectedGenresValues.Count; i++)
                    {
                        Genre genre = _dbcontext.Genres.Find(film.SelectedGenresValues[i]);
                        updatefilm.Genres.Add(genre);
                    }

                    for (int i = 0; i < updatefilm.Genres?.Count; i++)
                    {
                        FilmGenre filmgenre = new FilmGenre { Film = updatefilm, Genre = updatefilm.Genres[i] };
                        _dbcontext.FilmsGenres.Add(filmgenre);
                    }
                }

                filmRepository.UpdateFilm(updatefilm);


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("EditFilm", film);
            }
        }



        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpGet]
        public async Task<ActionResult> EditActor(int id)
        {
            Actor actor = await actorRepository.GetActorByIdAsync(id);
            return View("EditActor", actor);
        }



        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpPost]
        public ActionResult EditActor(Actor actor)
        {
            try
            {
                actorRepository.UpdateActor(actor);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("EditActor", actor);
            }
        }



        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpGet]
        public async Task<ActionResult> DeleteReview(int id)
        {
            try
            {
                var review = await _dbcontext.Reviews.FirstOrDefaultAsync(m => m.Id == id);


                filmRepository.DeleteReview(review);

                var film = await filmRepository.GetFilmByIdAsync(review.FilmId);

                return RedirectToAction("FilmDetails", film);
            }
            catch
            {
                return View(nameof(Index));
            }
        }



        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpGet]
        public ActionResult DeleteFilm(int id)
        {
            try
            {
                filmRepository.DeleteFilmById(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(nameof(Index));
            }
        }


        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpPost]
        public ActionResult DeleteActor(int id)
        {
            try
            {
                actorRepository.DeleteActorById(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(nameof(Index));
            }
        }







        [AllowAnonymous]
        public async Task<IActionResult> AddRoles()
        {
            //IdentityRole identityRole = new IdentityRole { Name = "Admin" };
            //IdentityRole identityRole2 = new IdentityRole { Name = "User" };

            //IdentityResult result = await roleManager.CreateAsync(identityRole);
            //IdentityResult result2 = await roleManager.CreateAsync(identityRole2);

            //if(result.Succeeded && result2.Succeeded)
            //{
            //    return RedirectToAction("Index", "Film");
            //}
            return View(nameof(Index));
        }
    }
}
