﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizinatorCore.Entities;
using QuizinatorCore.Entities.Exercises;
using QuizinatorCore.Services;
using QuizinatorCore.Interfaces;
using QuizinatorUI.ViewModels;

namespace QuizinatorUI.Controllers
{
    public class QuizzesController : Controller
    {
        private readonly IDatabaseService<Quiz> quizzesService;
        private readonly FileConverter fileConverter;

        //ctor
        public QuizzesController(IDatabaseService<Quiz> quizzesService, FileConverter fileConverter)
        {
            this.quizzesService = quizzesService;
            this.fileConverter = fileConverter;
        }

        public async Task<ActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["TitleSortParm"] = (sortOrder == "title_asc") ? "title_desc" : "title_asc";
            ViewData["ExercisesSortParm"] = (sortOrder == "exercises_asc") ? "exercises_desc" : "exercises_asc";
            ViewData["CollectionSizeSortParm"] = (sortOrder == "collectionSize_asc") ? "collectionSize_desc" : "collectionSize_asc";
            ViewData["CurrentFilter"] = searchString;

            IEnumerable<Quiz> quizzes = await quizzesService.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                quizzes = quizzes.Where(x => x.Title.Contains(searchString));
            }

            quizzes = sortOrder switch
            {
                "title_asc" => quizzes.OrderBy(x => x.Title),
                "title_desc" => quizzes.OrderByDescending(x => x.Title),
                _ => quizzes,
            };
            return View(quizzes);
        }

        public async Task<IEnumerable<Quiz>> Json()
        {
            return await quizzesService.GetAll();
        }

        public async Task<ActionResult> Details(Guid id)
        {
            Quiz quiz = (await quizzesService.GetAll()).First(x => x.QuizId == id);
            return View(quiz);
        }

        // (should be POST?)
        public ActionResult SubmitRating(Guid id, int rating)
        {
            quizzesService.AddRating(id, rating);
            return RedirectToAction(nameof(Details), new { id });
        }

        public ActionResult Create()
        {
            return RedirectToAction(nameof(Index), "Idioms");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Quiz newQuiz)
        {
            if (ModelState.IsValid)
            {
                quizzesService.Add(newQuiz);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(newQuiz);
            }
        }


        public async Task<ActionResult> Edit(Guid id)
        {
            Quiz quiz = (await quizzesService.GetAll()).First(x => x.QuizId == id);
            return View(quiz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, Quiz updatedQuiz)
        {
            if (ModelState.IsValid)
            {
                quizzesService.Replace(updatedQuiz);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(updatedQuiz);
            }
        }

        public async Task<ActionResult> Delete(Guid id)
        {
            Quiz model = (await quizzesService.GetAll()).First(x => x.QuizId == id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                quizzesService.Delete(id);
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
                Quiz[] newQuizzes = fileConverter.ConvertFileToObjects<Quiz>(model.MyFile);
                quizzesService.AddMultiple(newQuizzes);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
