using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizinatorUI.Services
{
    public enum ExerciseType
    {
        A, B, C, D, E
    }

    public class ExerciseFactory
    {
        public Dictionary<ExerciseType, string> ExerciseDescriptions = new Dictionary<ExerciseType, string>
        {
            {ExerciseType.A, "Translate the underlined words into Dutch." },
            {ExerciseType.B, "Complete the sentences with words from the box. Translate the words into English." },
            {ExerciseType.C, "Translate the word to complete the sentence." },
            {ExerciseType.D, "Translate the words into Dutch." },
            {ExerciseType.E, "Translate the words into English." }
        };
    }
}
