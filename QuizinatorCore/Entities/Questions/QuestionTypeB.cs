using QuizinatorCore.Entities.Idioms;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizinatorCore.Entities.Questions
{
    class QuestionTypeB : Question
    {
        // Type B: Pick a word from the box and translate.

        public string Translation { get; private set; }

        public QuestionTypeB(Idiom idiom) : base(idiom)
        {
            this.Translation = idiom.Translation;
        }

        protected override void SetQuestionText(Idiom idiom)
        {
            this.QuestionText = this.MakeGapInSentence(idiom.Sentence, idiom.Word);
        }

        protected override void SetAnswerText(Idiom idiom)
        {
            this.AnswerText = idiom.Word;
        }
    }
}
