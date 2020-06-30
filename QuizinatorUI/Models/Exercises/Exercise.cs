using QuizinatorUI.Services;
using System;
using System.Collections.Generic;
using QuizinatorUI.Models.Questions;

namespace QuizinatorUI.Models.Exercises
{
    public class Exercise
    {
        public Guid ExerciseId { get; set; }
        public ExerciseType Type { get; set; }
        public string Description { get; set; }
        public List<Question> Questions { get; set; }
    }
}