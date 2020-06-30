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

namespace QuizinatorUI.Controllers
{
    public class IdiomsController : Controller
    {
        private readonly IIdiomsDatabaseService idiomsService;
        private readonly FileConverter fileConverter;

        //ctor
        public IdiomsController(IIdiomsDatabaseService idiomsService, FileConverter fileConverter)
        {
            this.idiomsService = idiomsService;
            this.fileConverter = fileConverter;
        }

        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewData["WordSortParm"] = sortOrder == "word_asc" ? "word_desc" : "word_asc";
            ViewData["SentenceSortParm"] = sortOrder == "sentence_asc" ? "sentence_desc" : "sentence_asc";
            ViewData["TranslationSortParm"] = sortOrder == "translation_asc" ? "translation_desc" : "translation_asc";
            ViewData["UnitSortParm"] = sortOrder == "unit_asc" ? "unit_desc" : "unit_asc";
            ViewData["CurrentFilter"] = searchString;

            IEnumerable<Idiom> idioms = idiomsService.GetIdioms();

            if (!String.IsNullOrEmpty(searchString))
            {
                idioms = idioms.Where(x => x.Word.Contains(searchString)
                                        || x.Sentence.Contains(searchString)
                                        || x.Translation.Contains(searchString)
                                        || x.Unit.Contains(searchString));
            }

            idioms = sortOrder switch
            {
                "word_asc" => idioms.OrderBy(x => x.Word),
                "word_desc" => idioms.OrderByDescending(x => x.Word),
                "sentence_asc" => idioms.OrderBy(x => x.Sentence),
                "sentence_desc" => idioms.OrderByDescending(x => x.Sentence),
                "translation_asc" => idioms.OrderBy(x => x.Translation),
                "translation_desc" => idioms.OrderByDescending(x => x.Translation),
                "unit_asc" => idioms.OrderBy(x => x.Unit),
                "unit_desc" => idioms.OrderByDescending(x => x.Unit),
                _ => idioms,
            };
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