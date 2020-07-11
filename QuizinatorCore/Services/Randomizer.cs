using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuizinatorCore.Services
{
    public class Randomizer
    {
        public List<T> GetRandom<T>(List<T> list, int n)
        {
            List<T> shuffled = this.Shuffle<T>(list);
            List<T> selection = new List<T>();
            for (int i = 0; i < n; i++)
            {
                selection.Add(shuffled[i]);
            }
            return selection;
        }

        public List<int> GetRandomInts(int from, int to, int n)
        {
            List<int> all = new List<int>();
            for (int i = from; i < to; i++)
            {
                all.Add(i);
            }
            return this.GetRandom<int>(all, n);
        }

        public List<T> Shuffle<T>(List<T> list)
        {
            int currentIndex = list.Count;

            // While there remain elements to shuffle...
            while (currentIndex != 0)
            {
                T temporaryValue;
                int randomIndex;
                // Pick a remaining element...
                randomIndex = (new Random()).Next(currentIndex);
                currentIndex -= 1;
                // And swap it with the current element.
                temporaryValue = list[currentIndex];
                list[currentIndex] = list[randomIndex];
                list[randomIndex] = temporaryValue;
            }
            return list;
        }
    }
}

