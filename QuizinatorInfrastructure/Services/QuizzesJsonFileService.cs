using QuizinatorCore.Interfaces;
using QuizinatorCore.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using QuizinatorCore.Services;
using System.Threading.Tasks;

namespace QuizinatorInfrastructure.Services
{
    public class QuizzesJsonFileService : JsonFileService<Quiz>
    {
        //ctor
        public QuizzesJsonFileService()
        {
            this.JsonFileName = @"C:\Users\884573\Documents\Repositories\Quizinator_ASPNET\Data\quizzes.json";
        }

        protected override Guid GetId(Quiz quiz)
        {
            return quiz.QuizId;
        }

        protected override Quiz AddIdToNewItem(Quiz newQuiz)
        {
            newQuiz.QuizId = Guid.NewGuid();
            return newQuiz;
        }

        public async override Task AddRatingAsync(Guid quizId, int rating)
        {
            IEnumerable<Quiz> quizzes = await GetAllAsync();

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

            await UpdateSourceAsync(quizzes);
        }


    }
}