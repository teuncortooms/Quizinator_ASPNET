using QuizinatorCore.Entities;
using QuizinatorCore.Entities.Idioms;
using QuizinatorCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizinatorCore.Services
{
    public class IdiomSorter : ISorter<Idiom>
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
    }
}
