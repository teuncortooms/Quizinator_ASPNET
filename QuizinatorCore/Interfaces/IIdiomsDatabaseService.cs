using QuizinatorCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizinatorCore.Interfaces
{
    public interface IIdiomsDatabaseService
    {
        public IEnumerable<Idiom> GetIdioms();
        public void AddIdiom(Idiom idiom);
        public void AddIdioms(Idiom[] newIdioms);
        public void ReplaceIdiom(Idiom updatedIdiom);
        public void DeleteIdiom(Guid id);
        public void AddRating(Guid idiomId, int rating);
    }
}
