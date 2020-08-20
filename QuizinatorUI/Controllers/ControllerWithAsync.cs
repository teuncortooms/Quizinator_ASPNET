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

namespace QuizinatorUI.Controllers
{
    public abstract class ControllerWithAsync<T> : Controller
    {
        protected readonly IDatabaseService<T> dbService;
        protected readonly FileConverter fileConverter;
        protected readonly ISorter<T> sorter;

        //ctor
        public ControllerWithAsync(IDatabaseService<T> dbService, FileConverter fileConverter, ISorter<T> idiomSorter)
        {
            this.dbService = dbService;
            this.fileConverter = fileConverter;
            this.sorter = idiomSorter;
        }

        protected abstract Guid GetId(T x);
        protected abstract ViewDataDictionary SetSortandSearchViewParams(string sortOrder, string searchString, ViewDataDictionary ViewData);

        public async Task<ActionResult> Index(string sortOrder, string searchString)
        {
            Task<IEnumerable<T>> itemsTask = dbService.GetAllAsync();
            ViewData = SetSortandSearchViewParams(sortOrder, searchString, ViewData);
            IEnumerable<T> items = sorter.FilterAndSort(sortOrder, searchString, await itemsTask);
            return View(items);
        }

        public async Task<IEnumerable<T>> Json()
        {
            return await dbService.GetAllAsync();
        }

        public async Task<ActionResult> Details(Guid id)
        {
            T item = (await dbService.GetAllAsync()).First(x => GetId(x) == id);
            return View(item);
        }

        // (should be POST?)
        public async Task<ActionResult> SubmitRating(Guid id, int rating)
        {
            await dbService.AddRatingAsync(id, rating);
            return RedirectToAction(nameof(Details), new { id });
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Create(T newItem)
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

        public async Task<ActionResult> Edit(Guid id)
        {
            T item = (await dbService.GetAllAsync()).First(x => GetId(x) == id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, T updatedItem)
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

        public async Task<ActionResult> Delete(Guid id)
        {
            T model = (await dbService.GetAllAsync()).First(x => GetId(x) == id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id, IFormCollection collection)
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

        public ActionResult Import()
        {
            return View(new ImportFileViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Import(ImportFileViewModel model)
        {
            try
            {
                T[] newItems = fileConverter.ConvertFileToObjects<T>(model.MyFile);
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