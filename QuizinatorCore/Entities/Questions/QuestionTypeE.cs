using QuizinatorCore.Entities.Idioms;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizinatorCore.Entities.Questions
{
    class QuestionTypeE : Question
    {
        // Type E: "Translate the words into English."

        public QuestionTypeE(Idiom idiom) : base(idiom)
        {
        }

        protected override void SetQuestionText(Idiom idiom)
        {
            this.QuestionText = idiom.Translation;
        }

        protected override void SetAnswerText(Idiom idiom)
        {
            this.AnswerText = idiom.Word;
        }
    }
}
