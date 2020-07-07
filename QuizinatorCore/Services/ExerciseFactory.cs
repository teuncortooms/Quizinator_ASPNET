using QuizinatorCore.Entities.Exercises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizinatorCore.Services
{
    public enum ExerciseType
    {
        A, B, C, D, E
    }

    public class ExerciseFactory
    {
        public Dictionary<ExerciseType, string> ExerciseDescriptions { get; private set; } =
            new Dictionary<ExerciseType, string>
            {
                {ExerciseType.A, "Translate the underlined words into Dutch." },
                {ExerciseType.B, "Complete the sentences with words from the box. Translate the words into English." },
                {ExerciseType.C, "Translate the word to complete the sentence." },
                {ExerciseType.D, "Translate the words into Dutch." },
                {ExerciseType.E, "Translate the words into English." }
            };


        public static Exercise Create(ExerciseType type, int numberOfQuestions)
        {
            Exercise exercise;
            switch (type)
            {
                case ExerciseType.A:
                case ExerciseType.C:
                case ExerciseType.D:
                case ExerciseType.E:
                    exercise = new Exercise(type, numberOfQuestions);
                    break;
                case ExerciseType.B:
                    exercise = new ExerciseTypeB(type, numberOfQuestions);
                    break;
                default:
                    throw new Exception("Unknown exercise type: " + type);
            }
            return exercise;
        }
    }
}
