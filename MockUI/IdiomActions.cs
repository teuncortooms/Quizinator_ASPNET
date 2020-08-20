using QuizinatorCore.Entities;
using QuizinatorCore.Entities.Idioms;
using QuizinatorCore.Interfaces;
using QuizinatorCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MockUI
{
    class IdiomActions
    {
        private readonly IDatabaseService<Idiom> idiomsService;
        private readonly ISorter<Idiom> sorter;
        private bool isCanceled;

        //ctor
        public IdiomActions(IDatabaseService<Idiom> idiomsService, ISorter<Idiom> idiomsSorter)
        {
            this.idiomsService = idiomsService;
            this.sorter = idiomsSorter;
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
            Console.WriteLine("1. Show list of idioms");
            Console.WriteLine("2. Show details of an idiom");
            Console.WriteLine("3. Submit rating");
            Console.WriteLine("4. Add idiom");
            Console.WriteLine("5. Edit idiom");
            Console.WriteLine("6. Delete idiom");
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
                    this.ShowIdiom();
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

        public async void ShowList(string sortOrder = null, string searchString = null)
        {
            IEnumerable<Idiom> idioms = await idiomsService.GetAllAsync();
            idioms = sorter.FilterAndSort(sortOrder, searchString, idioms);
            foreach (Idiom idiom in idioms)
            {
                Console.WriteLine(idiom);
            }
        }

        public void ShowIdiom()
        {
            Console.WriteLine("Paste the Guid: ");
            Guid.TryParse(Console.ReadLine(), out Guid id);
            this.ShowIdiom(id);
        }

        public async void ShowIdiom(Guid id)
        {
            Idiom idiom = (await idiomsService.GetAllAsync()).First(x => x.IdiomId == id);
            Console.WriteLine(idiom);
        }

        public void SubmitRating(Guid id, int rating)
        {
            idiomsService.AddRatingAsync(id, rating);
            ShowIdiom(id);
        }

        public void Add()
        {
            Console.WriteLine("Word: ");
            string word = Console.ReadLine();
            Console.WriteLine("Translation: ");
            string translation = Console.ReadLine();
            Console.WriteLine("Unit: ");
            string unit = Console.ReadLine();
            Console.WriteLine("Sentence: ");
            string sentence = Console.ReadLine();
            this.Add(new Idiom()
            {
                IdiomId = Guid.NewGuid(),
                Word = word,
                Translation = translation,
                Unit = unit,
                Sentence = sentence
            });
        }

        public void Add(Idiom newIdiom)
        {
            idiomsService.AddAsync(newIdiom);
            Console.WriteLine("Idiom added: ");
            Console.WriteLine(newIdiom);
        }

        public void Edit(Idiom updatedIdiom)
        {
            try
            {
                idiomsService.ReplaceAsync(updatedIdiom);
                Console.WriteLine("Update succeeded: ");
                Console.WriteLine(updatedIdiom);
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
                idiomsService.DeleteAsync(id);
                Console.WriteLine("Deleted.");
            }
            catch
            {
                Console.WriteLine("deletion error");
            }
        }
    }
}
