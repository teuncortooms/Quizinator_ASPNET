using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuizinatorCore.Services
{
    public class Randomizer
    {
        public T[] GetRandom<T>(T[] array, int n)
        {
            List<T> shuffled = this.Shuffle<T>(array).ToList<T>();
            List<T> selection = new List<T>();
            for (int i = 0; i < n; i++)
            {
                selection.Add(shuffled[i]);
            }
            return selection.ToArray();
        }

        public int[] GetRandomInts(int from, int to, int n)
        {
            List<int> all = new List<int>();
            for (int i = from; i <= to; i++)
            {
                all.Add(i);
            }
            return this.GetRandom<int>(all.ToArray(), n);
        }

        public T[] Shuffle<T>(T[] array)
        {
            int currentIndex = array.Length;

            // While there remain elements to shuffle...
            while (currentIndex != 0)
            {
                T temporaryValue;
                int randomIndex;
                // Pick a remaining element...
                randomIndex = (new Random()).Next(currentIndex);
                currentIndex -= 1;
                // And swap it with the current element.
                temporaryValue = array[currentIndex];
                array[currentIndex] = array[randomIndex];
                array[randomIndex] = temporaryValue;
            }
            return array;
        }
    }
}

