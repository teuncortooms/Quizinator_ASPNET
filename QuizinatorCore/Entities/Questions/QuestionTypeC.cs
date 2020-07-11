using QuizinatorCore.Entities.Idioms;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizinatorCore.Entities.Questions
{
    class QuestionTypeC : Question
    {
        // Type C: Complete the sentence using a translation

        public QuestionTypeC(Idiom idiom) : base(idiom)
        {
        }

        protected override void SetQuestionText(Idiom idiom)
        {
            string gappedSentence = this.MakeGapInSentence(idiom.Sentence, idiom.Word);
            this.QuestionText = $"{gappedSentence} ({idiom.Translation})";
        }

        protected override void SetAnswerText(Idiom idiom)
        {
            this.AnswerText = idiom.Word;
        }
    }
}
