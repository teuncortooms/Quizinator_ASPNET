using Quizinator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quizinator.Services
{
    interface IIdiomsDatabaseService
    {
        public void AddIdiom(Idiom idiom);
        public void AddIdioms(Idiom[] newIdioms);
        public void ReplaceIdiom(Idiom updatedIdiom);
        public void AddRating(Guid idiomId, int rating);
    }
}
