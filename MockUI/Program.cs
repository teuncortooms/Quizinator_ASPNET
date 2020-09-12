using QuizinatorCore.Entities;
using QuizinatorCore.Services;
using QuizinatorInfrastructure.Repositories;
using System;

namespace MockUI
{
    class Program
    {
        static void Main(string[] args)
        {
            IdiomsRepository idiomsService = new IdiomsRepository();
            QuizzesRepository quizzesService = new QuizzesRepository();
            IdiomActions idiomActions = new IdiomActions(idiomsService, new IdiomSorter());
            QuizActions quizActions = new QuizActions(quizzesService, idiomsService, new QuizSorter());

            while (true)
            {
                PromptWithMainMenu(idiomActions, quizActions);
            }
        }

        private static void PromptWithMainMenu(IdiomActions idiomActions, QuizActions quizActions)
        {
            Console.WriteLine("Choose your menu: ");
            Console.WriteLine("1. Idioms");
            Console.WriteLine("2. Quizzes");
            Console.Write("Enter a number: ");
            int.TryParse(Console.ReadLine(), out int choice);

            switch (choice)
            {
                case 1:
                    idiomActions.PromptWithMenu();
                    break;
                case 2:
                    quizActions.PromptWithMenu();
                    break;
                default:
                    Console.WriteLine("Input not recognised.");
                    break;
            }
        }
    }
}
