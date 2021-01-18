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
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using QuizinatorCore.Entities.Idioms;
using Microsoft.Extensions.Logging;

namespace QuizinatorUI.Controllers
{
    [Route("[controller]/[action]")]
    public class IdiomsController : ControllerWithAsync<IdiomsController, Idiom>
    {
        public IdiomsController(ILogger<IdiomsController> logger,
                                IRepository<Idiom> repository,
                                FileConverter fileConverter,
                                ISorter<Idiom> idiomSorter)
            : base(logger, repository, fileConverter, idiomSorter)
        {
            logger.LogDebug(1, "NLog injected into IdiomsController");
        }

        protected override Guid GetId(Idiom x)
        {
            return x.IdiomId;
        }

        protected override ViewDataDictionary SetSortandSearchViewParams(string sortOrder, string searchString, ViewDataDictionary ViewData)
        {
            ViewData["WordSortParm"] = (sortOrder == "word_asc") ? "word_desc" : "word_asc";
            ViewData["SentenceSortParm"] = (sortOrder == "sentence_asc") ? "sentence_desc" : "sentence_asc";
            ViewData["TranslationSortParm"] = (sortOrder == "translation_asc") ? "translation_desc" : "translation_asc";
            ViewData["UnitSortParm"] = (sortOrder == "unit_asc") ? "unit_desc" : "unit_asc";
            ViewData["CurrentFilter"] = searchString;
            return ViewData;
        }
    }
}