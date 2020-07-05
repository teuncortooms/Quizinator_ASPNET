using Microsoft.AspNetCore.Mvc.ViewFeatures;
using QuizinatorCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizinatorUI.ViewServices
{
    public class IdiomsSorter
    {
        public IEnumerable<Idiom> FilterAndSort(string sortOrder, string searchString, IEnumerable<Idiom> idioms)
        {
            return this.Sort(sortOrder, this.Filter(searchString, idioms));
        }

        public IEnumerable<Idiom> Sort(string sortOrder, IEnumerable<Idiom> idioms)
        {
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
            return idioms;
        }

        public IEnumerable<Idiom> Filter(string searchString, IEnumerable<Idiom> idioms)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                idioms = idioms.Where(x => x.Word.Contains(searchString)
                                        || x.Sentence.Contains(searchString)
                                        || x.Translation.Contains(searchString)
                                        || x.Unit.Contains(searchString));
            }

            return idioms;
        }

        public ViewDataDictionary SetSortandSearchViewParams(string sortOrder, string searchString, ViewDataDictionary ViewData)
        {
            ViewData["WordSortParm"] = sortOrder == "word_asc" ? "word_desc" : "word_asc";
            ViewData["SentenceSortParm"] = sortOrder == "sentence_asc" ? "sentence_desc" : "sentence_asc";
            ViewData["TranslationSortParm"] = sortOrder == "translation_asc" ? "translation_desc" : "translation_asc";
            ViewData["UnitSortParm"] = sortOrder == "unit_asc" ? "unit_desc" : "unit_asc";
            ViewData["CurrentFilter"] = searchString;
            return ViewData;
        }
    }
}
