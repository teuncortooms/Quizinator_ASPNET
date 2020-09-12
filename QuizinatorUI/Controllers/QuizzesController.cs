using System;
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
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using AutoMapper;
using QuizinatorCore.Entities.Idioms;

namespace QuizinatorUI.Controllers
{
    public class QuizzesController : ControllerWithAsync<Quiz>
    {
        private readonly IRepository<Idiom> idiomsRepository;

        public QuizzesController(
            IRepository<Quiz> quizzesRepository,
            FileConverter fileConverter,
            ISorter<Quiz> idiomSorter,
            IRepository<Idiom> idiomsRepository
            )
            : base(quizzesRepository, fileConverter, idiomSorter)
        {
            this.idiomsRepository = idiomsRepository;
        }

        protected override Guid GetId(Quiz x)
        {
            return x.QuizId;
        }

        protected override ViewDataDictionary SetSortandSearchViewParams(
            string sortOrder, string searchString, ViewDataDictionary ViewData)
        {
            ViewData["TitleSortParm"] = (sortOrder == "title_asc") ? "title_desc" : "title_asc";
            ViewData["ExercisesSortParm"] = (sortOrder == "exercises_asc") ? "exercises_desc" : "exercises_asc";
            ViewData["CollectionSizeSortParm"] = (sortOrder == "collectionSize_asc") ? "collectionSize_desc" : "collectionSize_asc";
            ViewData["CurrentFilter"] = searchString;
            return ViewData;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(Quiz model)
        {
            if (ModelState.IsValid)
            {
                List<Idiom> idioms = (await idiomsRepository.GetAllAsync()).ToList();
                MapperConfiguration mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<Idiom, IdiomInCollection>());

                Quiz newQuiz = new Quiz(model.Title, new ExerciseFactory(), new Randomizer(), mapperConfig, idioms);

                await dbService.AddAsync(newQuiz);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }
    }
}
