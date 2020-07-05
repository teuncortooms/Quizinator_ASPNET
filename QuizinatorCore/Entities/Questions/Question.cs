using System;

namespace QuizinatorCore.Entities.Questions
{
    public class Question
    {
        public Guid QuestionId { get; set; }
        public Guid IdiomId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionAnswer { get; set; }
    }
}