using QuizinatorCore.Entities.Idioms;
using QuizinatorCore.Entities.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizinatorCore.Services
{
    public class QuestionFactory
    {
        public Question Create(ExerciseType type, Idiom idiom)
        {
            return type switch
            {
                ExerciseType.A => new QuestionTypeA(idiom),
                ExerciseType.B => new QuestionTypeB(idiom),
                ExerciseType.C => new QuestionTypeC(idiom),
                ExerciseType.D => new QuestionTypeD(idiom),
                ExerciseType.E => new QuestionTypeE(idiom),
                _ => throw new Exception("Unknown question type: " + type)
            };
        }
    }
}
