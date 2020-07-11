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
    public class QuizzesJsonFileService : IQuizzesDatabaseService
    {
        public FileConverter FileConverter { get; }

        private string JsonFileName
        {
            // FIXME:
            get { return @"C:\Users\884573\Documents\Repositories\Quizinator_ASPNET\Data\quizzes.json"; }
        }

        //ctor
        public QuizzesJsonFileService(FileConverter fileConverter)
        {
            FileConverter = fileConverter;
        }
        
        public IEnumerable<Quiz> GetQuizzes()
        {
            using StreamReader jsonFileReader = File.OpenText(JsonFileName);
            string json = jsonFileReader.ReadToEnd();
            return FileConverter.ConvertJsonToObjects<Quiz>(json);
        }

        public void AddQuiz(Quiz newQuiz)
        {
            IEnumerable<Quiz> quizzes = GetQuizzes();
            newQuiz.QuizId = Guid.NewGuid();
            quizzes = quizzes.Append(newQuiz);
            UpdateSource(quizzes);
        }

        public void AddQuizzes(Quiz[] newQuizzes)
        {
            IEnumerable<Quiz> quizzes = GetQuizzes();
            foreach (Quiz newQuiz in newQuizzes)
            {
                newQuiz.QuizId = Guid.NewGuid();
                quizzes = quizzes.Append(newQuiz);
            }
            UpdateSource(quizzes);
        }

        public void ReplaceQuiz(Quiz updatedQuiz)
        {
            List<Quiz> idioms = GetQuizzes().ToList();
            int index = idioms.FindIndex(x => x.QuizId == updatedQuiz.QuizId);
            idioms[index] = updatedQuiz;
            UpdateSource(idioms);
        }

        public void DeleteQuiz(Guid id)
        {
            List<Quiz> quizzes = GetQuizzes().ToList();
            quizzes.RemoveAll(x => x.QuizId == id);
            UpdateSource(quizzes);
        }

        public void AddRating(Guid id, int rating)
        {
            IEnumerable<Quiz> quizzes = GetQuizzes();

            Quiz quiz = quizzes.First(x => x.QuizId == id);

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

        private void UpdateSource(IEnumerable<Quiz> quizzes)
        {
            using FileStream jsonFile = File.Open(JsonFileName, FileMode.Create);
            Utf8JsonWriter writer = new Utf8JsonWriter(jsonFile, new JsonWriterOptions { Indented = true });
            JsonSerializer.Serialize(writer, quizzes);
        }
    }
}