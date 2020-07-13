using QuizinatorCore.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using QuizinatorCore.Services;
using QuizinatorCore.Interfaces;
using QuizinatorCore.Entities.Idioms;

namespace QuizinatorInfrastructure.Services
{
    public class IdiomsJsonFileService : JsonFileService<Idiom>
    {
        protected readonly new string JsonFileName = @"C:\Users\884573\Documents\Repositories\Quizinator_ASPNET\Data\idioms.json";

        protected override Guid GetId(Idiom idiom)
        {
            return idiom.IdiomId;
        }

        protected override Idiom AddIdToItem(Idiom newItem)
        {
            newItem.IdiomId = Guid.NewGuid();
            return newItem;
        }

        public async override void AddRating(Guid idiomId, int rating)
        {
            IEnumerable<Idiom> idioms = await GetAll();

            Idiom idiom = idioms.First(x => GetId(x) == idiomId);

            if (idiom.Ratings == null)
            {
                idiom.Ratings = new int[] { rating };
            }
            else
            {
                List<int> ratings = idiom.Ratings.ToList();
                ratings.Add(rating);
                idiom.Ratings = ratings.ToArray();
            }

            UpdateSource(idioms);
        }
    }
}

