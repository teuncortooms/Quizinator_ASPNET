using AutoMapper;
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
        public Guid QuizId { get; set; }
        public string Title { get; set; }
        public List<Exercise> Exercises { get; set; }
        public List<IdiomInCollection> IdiomsCollection { get; set; }
        public int[] Ratings { get; set; }

        private readonly ExerciseFactory exerciseFactory;
        private readonly Randomizer randomizer;
        private readonly MapperConfiguration mapperConfig;

        public Quiz()
        {
        }

        public Quiz(
            string title,
            ExerciseFactory factory,
            Randomizer randomizer,
            MapperConfiguration mapperConfig,
            List<Idiom> idioms = null
            )
        {
            this.QuizId = Guid.NewGuid();
            this.Title = title;
            this.Exercises = new List<Exercise>();
            this.IdiomsCollection = new List<IdiomInCollection>();
            this.exerciseFactory = factory;
            this.randomizer = randomizer;
            this.mapperConfig = mapperConfig;

            if (idioms != null)
            {
                AddToIdiomsCollection(idioms);
                AddAutoExercises(3, 5);
            }
        }

        private void AddAutoExercises(int maxNumberOfExercises, int numberOfQuestionsPerExercise)
        {
            int currentExercise = 1;
            int idiomsRemaining = this.IdiomsCollection.Count;
            ExerciseType type = ExerciseType.A;
            while (currentExercise <= maxNumberOfExercises &&
                    numberOfQuestionsPerExercise < idiomsRemaining &&
                    TypeExists(type))
            {
                AddExerciseWithRandomQuestions(type, numberOfQuestionsPerExercise);
                idiomsRemaining -= numberOfQuestionsPerExercise;
                type++;
            }
        }

        private bool TypeExists(ExerciseType type)
        {
            return Enum.IsDefined(typeof(ExerciseType), type);
        }

        public void AddToIdiomsCollection(IEnumerable<Idiom> idioms)
        {
            IEnumerable<IdiomInCollection> newIdiomsInCollection = MapToIdiomInCollection(idioms);
            this.IdiomsCollection.AddRange(newIdiomsInCollection);
        }

        private IEnumerable<IdiomInCollection> MapToIdiomInCollection(IEnumerable<Idiom> idioms)
        {
            var mapper = this.mapperConfig.CreateMapper();
            return idioms.ToList().Select(x => mapper.Map<IdiomInCollection>(x));
        }

        private void AddExerciseWithRandomQuestions(ExerciseType type, int numberOfQuestions)
        {
            List<IdiomInCollection> selection = randomizer.GetRandom<IdiomInCollection>(
                this.IdiomsCollection.Where(x => x.IsAvailable).ToList(),
                numberOfQuestions);
            this.AddExercise(type, selection);
            selection.ForEach(o => o.IsAvailable = false);
        }

        private void AddExercise(ExerciseType type, List<IdiomInCollection> idioms)
        {
            this.Exercises.Add(exerciseFactory.Create(type, idioms));
        }

        public override string ToString()
        {
            int exercisesCount = Exercises != null ? Exercises.Count : 0;
            int idiomsCount = IdiomsCollection != null ? IdiomsCollection.Count : 0;
            return string.Format("{0, -40} {1, -30} {2, -15} {3, -25}",
                QuizId, Title, $"{exercisesCount} exercises", $"{idiomsCount} idioms in collection");
        }

        public string GetDetails()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Id: {QuizId}")
            .AppendLine($"Title: {Title}")
            .AppendLine()
            .AppendLine($"Exercises:")
            .AppendLine();
            foreach (var exercise in Exercises)
            {
                builder.AppendLine(exercise.ToString());
            }
            builder.AppendLine()
            .AppendLine($"Idiom Collection:")
            .AppendLine();
            foreach (var idiom in IdiomsCollection)
            {
                builder.AppendLine(idiom.ToString());
            }
            return builder.ToString();
        }
    }
}
