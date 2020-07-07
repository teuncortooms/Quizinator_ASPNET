using System;

namespace QuizinatorCore.Entities.Questions
{
    public class Question
    {
        public Guid QuestionId { get; private set; }
        public Guid IdiomId { get; private set; }
        public string QuestionText { get; private set; }
        public string QuestionAnswer { get; private set; }
    }
}