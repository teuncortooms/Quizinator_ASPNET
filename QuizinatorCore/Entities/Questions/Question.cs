using QuizinatorCore.Entities.Idioms;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace QuizinatorCore.Entities.Questions
{
    public abstract class Question
    {
        public Guid QuestionId { get; set; }
        public Guid IdiomId { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }

        public Question()
        {

        }

        public Question(Idiom idiom)
        {
            this.QuestionId = Guid.NewGuid();
            this.IdiomId = idiom.IdiomId;
            this.SetQuestionText(idiom);
            this.SetAnswerText(idiom);
        }

        protected abstract void SetQuestionText(Idiom idiom);
        protected abstract void SetAnswerText(Idiom idiom);

        protected string MakeGapInSentence(string sentence, string word)
        {
            Regex regEx = new Regex(word, RegexOptions.IgnoreCase);
            return regEx.Replace(sentence, "________");
        }
    }
}