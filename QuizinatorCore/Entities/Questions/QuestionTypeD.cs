using QuizinatorCore.Entities.Idioms;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizinatorCore.Entities.Questions
{
    class QuestionTypeD : Question
    {
        // Type D: "Translate the words into Dutch."

        public QuestionTypeD(Idiom idiom) : base(idiom)
        {
        }

        protected override void SetQuestionText(Idiom idiom)
        {
            this.QuestionText = idiom.Word;
        }

        protected override void SetAnswerText(Idiom idiom)
        {
            this.AnswerText = idiom.Translation;
        }
    }
}
