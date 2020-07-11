using QuizinatorCore.Entities.Idioms;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizinatorCore.Entities.Questions
{
    public class QuestionTypeA : Question
    {
        // Type A: Translate the underlined word

        public QuestionTypeA(Idiom idiom) : base(idiom)
        {
        }

        protected override void SetQuestionText(Idiom idiom)
        {
            this.QuestionText = $"{idiom.Sentence} ({idiom.Word})";
        }

        protected override void SetAnswerText(Idiom idiom)
        {
            this.AnswerText = idiom.Translation;
        }
    }
}
