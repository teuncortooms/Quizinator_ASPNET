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
    public class IdiomsJsonFileService : IIdiomsDatabaseService
    {
        public FileConverter FileConverter { get; }

        private string JsonFileName
        {
            // FIXME:
            get { return @"C:\Users\884573\Documents\Repositories\Quizinator_ASPNET\Data\idioms.json"; }
        }

        //ctor
        public IdiomsJsonFileService(FileConverter fileConverter)
        {
            this.FileConverter = fileConverter;
        }

        public IEnumerable<Idiom> GetIdioms()
        {
            using StreamReader jsonFileReader = File.OpenText(JsonFileName);
            string json = jsonFileReader.ReadToEnd();
            return FileConverter.ConvertJsonToObjects<Idiom>(json);
        }

        public void AddIdiom(Idiom idiom)
        {
            IEnumerable<Idiom> idioms = GetIdioms();
            idiom.IdiomId = Guid.NewGuid();
            idioms = idioms.Append(idiom);
            UpdateSource(idioms);
        }

        public void AddIdioms(Idiom[] newIdioms)
        {
            IEnumerable<Idiom> idioms = GetIdioms();
            foreach (Idiom newIdiom in newIdioms)
            {
                newIdiom.IdiomId = Guid.NewGuid();
                idioms = idioms.Append(newIdiom);
            }
            UpdateSource(idioms);
        }

        public void ReplaceIdiom(Idiom updatedIdiom)
        {
            List<Idiom> idioms = GetIdioms().ToList();
            int index = idioms.FindIndex(x => x.IdiomId == updatedIdiom.IdiomId);
            idioms[index] = updatedIdiom;
            UpdateSource(idioms);
        }

        public void DeleteIdiom(Guid id)
        {
            List<Idiom> idioms = GetIdioms().ToList();
            idioms.RemoveAll(x => x.IdiomId == id);
            UpdateSource(idioms);
        }

        public void AddRating(Guid idiomId, int rating)
        {
            IEnumerable<Idiom> idioms = GetIdioms();

            Idiom idiom = idioms.First(x => x.IdiomId == idiomId);

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

        private void UpdateSource(IEnumerable<Idiom> idioms)
        {
            using FileStream jsonFile = File.Open(JsonFileName, FileMode.Create);
            Utf8JsonWriter writer = new Utf8JsonWriter(jsonFile, new JsonWriterOptions { Indented = true });
            JsonSerializer.Serialize(writer, idioms);
        }
    }
}

