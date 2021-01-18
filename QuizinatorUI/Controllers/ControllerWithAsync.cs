using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using QuizinatorUI.ViewModels;
using QuizinatorCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizinatorCore.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace QuizinatorUI.Controllers
{
    [Route("[controller]/[action]")]
    public abstract class ControllerWithAsync<TController, TModel> : Controller
    {
        protected readonly ILogger logger;

        protected readonly IRepository<TModel> dbService;
        protected readonly FileConverter fileConverter;
        protected readonly ISorter<TModel> sorter;

        //ctor
        public ControllerWithAsync(ILogger<TController> logger,
                                   IRepository<TModel> repository,
                                   FileConverter fileConverter,
                                   ISorter<TModel> idiomSorter)
        {
            this.logger = logger;
            logger.LogDebug(1, "NLog injected into AsyncController");
            this.dbService = repository;
            this.fileConverter = fileConverter;
            this.sorter = idiomSorter;
        }

        protected abstract Guid GetId(TModel x);
        protected abstract ViewDataDictionary SetSortandSearchViewParams(string sortOrder, string searchString, ViewDataDictionary ViewData);

        [Route("/[controller]")]
        [Route("~/[controller]/[action]")]
        [HttpGet]
        public async Task<ActionResult> Index(string sortOrder, string searchString)
        {
            logger.LogInformation("Hello, this is the index!");

            IEnumerable<TModel> items = new List<TModel>();
            try
            {
                Task<IEnumerable<TModel>> itemsTask = dbService.GetAllAsync();
                ViewData = SetSortandSearchViewParams(sortOrder, searchString, ViewData);
                items = sorter.FilterAndSort(sortOrder, searchString, await itemsTask);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "testing error logging");
            }
            return View(items);
        }

        [HttpGet]
        public void LogSomething(string something)
        {
            logger.LogError(something);
        }

        [HttpGet]
        public async Task<IEnumerable<TModel>> Json()
        {
            return await dbService.GetAllAsync();
        }

        [HttpGet]
        public async Task<ActionResult> Details(Guid id)
        {
            TModel item = (await dbService.GetAllAsync()).First(x => GetId(x) == id);
            return View(item);
        }
        
        [HttpGet] // (should be POST?)
        public async Task<ActionResult> SubmitRating(Guid id, int rating)
        {
            await dbService.AddRatingAsync(id, rating);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Create([FromBody]TModel newItem)
        {
            if (ModelState.IsValid)
            {
                await dbService.AddAsync(newItem);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            TModel item = (await dbService.GetAllAsync()).First(x => GetId(x) == id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, [FromBody]TModel updatedItem)
        {
            try
            {
                await dbService.ReplaceAsync(updatedItem);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // report error
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            TModel model = (await dbService.GetAllAsync()).First(x => GetId(x) == id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id, [FromBody]IFormCollection collection)
        {
            try
            {
                await dbService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Import()
        {
            return View(new ImportFileViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Import([FromBody]ImportFileViewModel model)
        {
            try
            {
                TModel[] newItems = fileConverter.ConvertFileToObjects<TModel>(model.MyFile);
                await dbService.AddMultipleAsync(newItems);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}