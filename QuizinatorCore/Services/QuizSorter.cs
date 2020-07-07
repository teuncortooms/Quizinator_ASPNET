using QuizinatorCore.Entities;
using QuizinatorCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizinatorCore.Services
{
    public class QuizSorter : ISorter<Quiz>
    {
        public IEnumerable<Quiz> FilterAndSort(string sortOrder, string searchString, IEnumerable<Quiz> quizzes)
        {
            return this.Sort(sortOrder, this.Filter(searchString, quizzes));
        }

        public IEnumerable<Quiz> Sort(string sortOrder, IEnumerable<Quiz> quizzes)
        {
            quizzes = sortOrder switch
            {
                "title_asc" => quizzes.OrderBy(x => x.Title),
                "title_desc" => quizzes.OrderByDescending(x => x.Title),
                _ => quizzes,
            };
            return quizzes;
        }

        public IEnumerable<Quiz> Filter(string searchString, IEnumerable<Quiz> quizzes)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                quizzes = quizzes.Where(x => x.Title.Contains(searchString));
            }
            return quizzes;
        }
    }
}
