using QuizinatorCore.Entities;
using QuizinatorCore.Services;
using QuizinatorInfrastructure.Services;
using System;

namespace MockUI
{
    class Program
    {
        static void Main(string[] args)
        {
            FileConverter fileConverter = new FileConverter();
            IdiomActions idiomActions = new IdiomActions(new IdiomsJsonFileService(fileConverter), new IdiomSorter());
            QuizActions quizActions = new QuizActions(new QuizzesJsonFileService(fileConverter), new QuizSorter());

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
