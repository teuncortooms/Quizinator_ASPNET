using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AnotherMVCProject.Models;
using AnotherMVCProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnotherMVCProject.Controllers
{
    public class IdiomsController : Controller
    {
        private readonly IdiomsJsonFileService idiomsService;
        private readonly XToIdiomConverter idiomsConverter;
        private object _context;

        //ctor
        public IdiomsController(IdiomsJsonFileService idiomsService, XToIdiomConverter idiomsConverter)
        {
            this.idiomsService = idiomsService;
            this.idiomsConverter = idiomsConverter;
        }

        public ActionResult Index(string sortOrder)
        {
            ViewData["WordSortParm"] = sortOrder == "word_asc" ? "word_desc" : "word_asc";
            ViewData["SentenceSortParm"] = sortOrder == "sentence_asc" ? "sentence_desc" : "sentence_asc";
            ViewData["TranslationSortParm"] = sortOrder == "translation_asc" ? "translation_desc" : "translation_asc";
            ViewData["UnitSortParm"] = sortOrder == "unit_asc" ? "unit_desc" : "unit_desc";

            IEnumerable<Idiom> idioms = idiomsService.GetIdioms();
            switch (sortOrder)
            {
                case "word_asc":
                    idioms = idioms.OrderBy(x => x.Word);
                    break;
                case "word_desc":
                    idioms = idioms.OrderByDescending(x => x.Word);
                    break;
                case "sentence_asc":
                    idioms = idioms.OrderBy(x => x.Sentence);
                    break;
                case "sentence_desc":
                    idioms = idioms.OrderByDescending(x => x.Sentence);
                    break;
                case "translation_asc":
                    idioms = idioms.OrderBy(x => x.Translation);
                    break;
                case "translation_desc":
                    idioms = idioms.OrderByDescending(x => x.Translation);
                    break;
                case "unit_asc":
                    idioms = idioms.OrderBy(x => x.Unit);
                    break;
                case "unit_desc":
                    idioms = idioms.OrderByDescending(x => x.Unit);
                    break;
                default:
                    idioms = idioms.OrderBy(x => x.IdiomId);
                    break;
            }
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
            try
            {
                idiomsService.AddIdiom(newIdiom);
                return RedirectToAction(nameof(Index));
            }
            catch
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
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

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
                Idiom[] newIdioms = idiomsConverter.ConvertFileToIdioms(model.MyFile);
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