using QuizinatorUI.Models.Exercises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizinatorUI.Models
{
    public class Quiz
    {
        public Guid QuizId { get; set; }
        public string Title { get; set; }
        public List<Exercise> Exercises { get; set; }
        public int[] Ratings { get; set; }
    }
}
