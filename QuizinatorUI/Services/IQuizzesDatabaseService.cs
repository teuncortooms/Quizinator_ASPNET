﻿using QuizinatorUI.Models;
using System;
using System.Collections.Generic;

namespace QuizinatorUI
{
    public interface IQuizzesDatabaseService
    {
        public IEnumerable<Quiz> GetQuizzes();
        public void AddQuiz(Quiz newQuiz);
        public void AddQuizzes(Quiz[] newQuizzes);
        public void ReplaceQuiz(Quiz updatedQuiz);
        public void DeleteQuiz(Guid id);
        public void AddRating(Guid id, int rating);

    }
}