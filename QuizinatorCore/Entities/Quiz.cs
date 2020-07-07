using QuizinatorCore.Entities.Exercises;
using QuizinatorCore.Entities.Idioms;
using QuizinatorCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizinatorCore.Entities
{
    public class Quiz
    {
        public Guid QuizId { get; private set; }
        public string Title { get; private set; }
        public List<Exercise> Exercises { get; private set; }
        public List<IdiomInCollection> IdiomsCollection { get; private set; }
        public int[] Ratings { get; private set; }

        private readonly Randomizer randomizer;

        public Quiz(Randomizer randomizer, List<IdiomInCollection> idiomsCollection = null)
        {
            this.randomizer = randomizer;
            if (idiomsCollection != null)
            {
                this.IdiomsCollection = idiomsCollection;
                AddExerciseWithRandomQuestions(ExerciseType.A, 5);
                AddExerciseWithRandomQuestions(ExerciseType.B, 5);
                AddExerciseWithRandomQuestions(ExerciseType.C, 5);
            }

        }

        private void AddExerciseWithRandomQuestions(ExerciseType type, int numberOfQuestions)
        {
            List<IdiomInCollection> idioms = randomizer.GetRandom<IdiomInCollection>(
                this.IdiomsCollection.ToArray(),
                numberOfQuestions
            ).ToList<IdiomInCollection>();
            this.AddExercise(type, idioms);
        }

        private void AddExercise(ExerciseType type, List<IdiomInCollection> idioms)
        {
            this.Exercises.Add(ExerciseFactory.Create(type, idioms));
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"{QuizId,-40}")
                .Append($"{Title,-30}");
            return builder.ToString();
        }
    }
}
