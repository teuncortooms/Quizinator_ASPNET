using Quizinator.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Quizinator.Services
{
    public class IdiomsJsonFileService : IIdiomsDatabaseService
    {
        public IWebHostEnvironment WebHostEnvironment { get; }
        public XToIdiomConverter IdiomsConverter { get; }

        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "idioms.json"); }
        }

        //ctor
        public IdiomsJsonFileService(IWebHostEnvironment webHostEnvironment, XToIdiomConverter idiomsConverter)
        {
            WebHostEnvironment = webHostEnvironment;
            IdiomsConverter = idiomsConverter;
        }

        public IEnumerable<Idiom> GetIdioms()
        {
            using StreamReader jsonFileReader = File.OpenText(JsonFileName);
            string json = jsonFileReader.ReadToEnd();
            return IdiomsConverter.ConvertJsonToIdioms(json);
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

        internal void DeleteIdiom(Guid id)
        {
            List<Idiom> idioms = GetIdioms().ToList();
            idioms.RemoveAll(x => x.IdiomId == id);
            UpdateSource(idioms);
        }
    }
}
