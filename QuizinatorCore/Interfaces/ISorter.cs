using System;
using System.Collections.Generic;
using System.Text;

namespace QuizinatorCore.Interfaces
{
    public interface ISorter<T>
    {
        public IEnumerable<T> FilterAndSort(string sortOrder, string searchString, IEnumerable<T> items);
        public IEnumerable<T> Sort(string sortOrder, IEnumerable<T> items);
        public IEnumerable<T> Filter(string searchString, IEnumerable<T> items);
    }
}
