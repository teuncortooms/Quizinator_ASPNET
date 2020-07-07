using QuizinatorCore.Entities;
using QuizinatorCore.Interfaces;
using QuizinatorCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MockUI
{
    class QuizActions
    {
        private readonly IQuizzesDatabaseService quizzesService;
        private readonly ISorter<Quiz> sorter;
        private bool isCanceled;

        //ctor
        public QuizActions(IQuizzesDatabaseService quizzesService, ISorter<Quiz> sorter)
        {
            this.quizzesService = quizzesService;
            this.sorter = sorter;
        }

        public void PromptWithMenu()
        {
            while (!isCanceled)
            {
                ShowMenu();
                int.TryParse(Console.ReadLine(), out int choice);
                Execute(choice);
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("Choose your action: ");
            Console.WriteLine("1. Show list of quizzes");
            Console.WriteLine("2. Show details of a quiz");
            Console.WriteLine("3. Submit rating");
            Console.WriteLine("4. Add quiz");
            Console.WriteLine("5. Edit quiz");
            Console.WriteLine("6. Delete quiz");
            Console.WriteLine("9. Cancel");
        }

        private void Execute(int choice)
        {
            switch (choice)
            {
                case 1:
                    this.ShowList();
                    break;
                case 2:
                    this.ShowQuiz();
                    break;
                case 3:
                    Console.WriteLine("Rating not yet implemented");
                    break;
                case 4:
                    this.Add();
                    break;
                case 5:
                    Console.WriteLine("Edit not yet implemented");
                    break;
                case 6:
                    Console.WriteLine("Delete not yet implemented");
                    break;
                case 9:
                    this.isCanceled = true;
                    break;
                default:
                    Console.WriteLine("Action not recognised. Make sure to type a number only.");
                    break;
            }
        }

        public void ShowList(string sortOrder = null, string searchString = null)
        {
            IEnumerable<Quiz> quizzes = quizzesService.GetQuizzes();
            quizzes = sorter.FilterAndSort(sortOrder, searchString, quizzes);
            foreach (Quiz quiz in quizzes)
            {
                Console.WriteLine(quiz);
            }
        }

        public void ShowQuiz()
        {
            Console.WriteLine("Paste the Guid: ");
            Guid.TryParse(Console.ReadLine(), out Guid id);
            this.ShowQuiz(id);
        }

        public void ShowQuiz(Guid id)
        {
            Quiz quiz = quizzesService.GetQuizzes().First(x => x.QuizId == id);
            Console.WriteLine(quiz);
        }

        public void SubmitRating(Guid id, int rating)
        {
            quizzesService.AddRating(id, rating);
            ShowQuiz(id);
        }

        public void Add()
        {
            Console.WriteLine("Title: ");
            string title = Console.ReadLine();
            this.Add(new Quiz()
            {
                QuizId = new Guid(),
                Title = title,
            });
        }

        public void Add(Quiz newQuiz)
        {
            quizzesService.AddQuiz(newQuiz);
            Console.WriteLine("Quiz added: ");
            Console.WriteLine(newQuiz);
        }

        public void Edit(Quiz updatedQuiz)
        {
            try
            {
                quizzesService.ReplaceQuiz(updatedQuiz);
                Console.WriteLine("Update succeeded: ");
                Console.WriteLine(updatedQuiz);
            }
            catch
            {
                Console.WriteLine("error");
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                quizzesService.DeleteQuiz(id);
                Console.WriteLine("Deleted.");
            }
            catch
            {
                Console.WriteLine("deletion error");
            }
        }
    }
}
