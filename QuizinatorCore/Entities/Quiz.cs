using QuizinatorCore.Entities.Exercises;
using QuizinatorCore.Entities.Idioms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizinatorCore.Entities
{
    public class Quiz
    {
        public Guid QuizId { get; set; }
        public string Title { get; set; }
        public List<Exercise> Exercises { get; set; }
        public List<IdiomInCollection> IdiomsCollection { get; set; }
        public int[] Ratings { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"{QuizId,-40}")
                .Append($"{Title,-30}");
            return builder.ToString();
        }
    }
}
