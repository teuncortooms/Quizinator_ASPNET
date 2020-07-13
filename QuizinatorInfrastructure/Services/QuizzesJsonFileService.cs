using QuizinatorCore.Interfaces;
using QuizinatorCore.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using QuizinatorCore.Services;

namespace QuizinatorInfrastructure.Services
{
    public class QuizzesJsonFileService : JsonFileService<Quiz>
    {
        protected readonly new string JsonFileName = @"C:\Users\884573\Documents\Repositories\Quizinator_ASPNET\Data\quizzes.json";

        protected override Guid GetId(Quiz quiz)
        {
            return quiz.QuizId;
        }

        protected override Quiz AddIdToItem(Quiz newQuiz)
        {
            newQuiz.QuizId = Guid.NewGuid();
            return newQuiz;
        }

        public async override void AddRating(Guid quizId, int rating)
        {
            IEnumerable<Quiz> quizzes = await GetAll();

            Quiz quiz = quizzes.First(x => GetId(x) == quizId);

            if (quiz.Ratings == null)
            {
                quiz.Ratings = new int[] { rating };
            }
            else
            {
                List<int> ratings = quiz.Ratings.ToList();
                ratings.Add(rating);
                quiz.Ratings = ratings.ToArray();
            }

            UpdateSource(quizzes);
        }


    }
}