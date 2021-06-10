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

namespace Kinomania.Controllers
{
    public class FilmController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;

        public FilmController(ApplicationDbContext dbContext)
        {
            this._dbcontext = dbContext;
        }

        // GET: FilmController
        public ActionResult Index()
        {
            return View(_dbcontext.Films.ToList());
        }

        public IActionResult AddFilm()
        {
            ViewBag.Genres = _dbcontext.Genres.Select(x=>x.Name).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFilm( [Bind("Name,ReleaseDate,Producer,Budget,PosterFile")] Film film,[Bind("Genres")] List<SelectListItem> Genres)
        {
            string imgext = Path.GetExtension(film.PosterFile.FileName);
            if (imgext == ".png" || imgext == ".jpg")
            {
                FilmRepository filmrepo = new FilmRepository(_dbcontext);
                var imgpath = Path.Combine(filmrepo.ImagesfolderPath, film.PosterFile.FileName);
                var stream = new FileStream(imgpath, FileMode.Create);
                await film.PosterFile.CopyToAsync(stream);
                film.PosterPath = imgpath;
                filmrepo.AddNewFilm(film, film.Actors, film.Genres);
            }

            return RedirectToAction(nameof(Index));
        }


        public IActionResult AddActor()
        {
            ViewBag.Genres = _dbcontext.Genres.Select(x => x.Name).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddActor([Bind("FirstName,LastName,Birthday,PhotoFile")] Actor actor)
        {
            string imgext = Path.GetExtension(actor.PhotoFile.FileName);
            if (imgext == ".png" || imgext == ".jpg")
            {
                ActorRepository actorrepo = new ActorRepository(_dbcontext);
                var imgpath = Path.Combine(actorrepo.ImagesfolderPath, actor.PhotoFile.FileName);
                var stream = new FileStream(imgpath, FileMode.Create);
                await actor.PhotoFile.CopyToAsync(stream);
                actor.PhotoPath = imgpath;
                actorrepo.AddNewActor(actor, actor.Films);
            }

            return RedirectToAction(nameof(Index));
        }



        // GET: FilmController/Details/5
        public ActionResult Details(int id)
        {
            return View(_dbcontext.Films.Find(id));
        }


        // POST: FilmController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FilmController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FilmController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FilmController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FilmController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
