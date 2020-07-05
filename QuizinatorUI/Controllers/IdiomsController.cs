using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using QuizinatorCore.Entities;
using QuizinatorUI.ViewModels;
using QuizinatorCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizinatorCore.Services;
using QuizinatorUI.ViewServices;

namespace QuizinatorUI.Controllers
{
    public class IdiomsController : Controller
    {
        private readonly IIdiomsDatabaseService idiomsService;
        private readonly FileConverter fileConverter;
        private readonly IdiomsSorter sorter;

        //ctor
        public IdiomsController(IIdiomsDatabaseService idiomsService, FileConverter fileConverter, IdiomsSorter idiomsSorter)
        {
            this.idiomsService = idiomsService;
            this.fileConverter = fileConverter;
            this.sorter = idiomsSorter;
        }

        public ActionResult Index(string sortOrder, string searchString)
        {
            IEnumerable<Idiom> idioms = idiomsService.GetIdioms();
            idioms = sorter.FilterAndSort(sortOrder, searchString, idioms);
            ViewData = sorter.SetSortandSearchViewParams(sortOrder, searchString, ViewData);
            return View(idioms);
        }

 

        public IEnumerable<Idiom> Json()
        {
            return idiomsService.GetIdioms();
        }

        public ActionResult Details(Guid id)
        {
            Idiom idiom = idiomsService.GetIdioms().First(x => x.IdiomId == id);
            return View(idiom);
        }

        // (should be POST?)
        public ActionResult SubmitRating(Guid id, int rating)
        {
            idiomsService.AddRating(id, rating);
            return RedirectToAction(nameof(Details), new { id });
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Idiom newIdiom)
        {
            if (ModelState.IsValid)
            {
                idiomsService.AddIdiom(newIdiom);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        public ActionResult Edit(Guid id)
        {
            Idiom idiom = idiomsService.GetIdioms().First(x => x.IdiomId == id);
            return View(idiom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, Idiom updatedIdiom)
        {
            try
            {
                idiomsService.ReplaceIdiom(updatedIdiom);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // report error
                return View();
            }
        }

        public ActionResult Delete(Guid id)
        {
            Idiom model = idiomsService.GetIdioms().First(x => x.IdiomId == id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                idiomsService.DeleteIdiom(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Import()
        {
            return View(new ImportFileViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import(ImportFileViewModel model)
        {
            try
            {
                Idiom[] newIdioms = fileConverter.ConvertFileToObjects<Idiom>(model.MyFile);
                idiomsService.AddIdioms(newIdioms);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}