using QuizinatorCore.Entities;
using QuizinatorCore.Entities.Exercises;
using QuizinatorCore.Entities.Idioms;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizinatorCore.Services
{
    class QuizAuthor
    {
        private readonly Quiz quiz;

        public QuizAuthor(Quiz quiz, List<IdiomInCollection> idiomsCollection = null)
        {
            this.quiz = quiz;
            if (idiomsCollection != null)
            {
                this.quiz.IdiomsCollection = idiomsCollection;
                AddExercise(ExerciseType.A, 5);
                AddExercise(ExerciseType.B, 5);
                AddExercise(ExerciseType.C, 5);
            }
            
        }

        private void AddExercise(ExerciseType type, int numberOfQuestions)
        {
            Exercise exercise = ExerciseFactory.
        }
    }
}
