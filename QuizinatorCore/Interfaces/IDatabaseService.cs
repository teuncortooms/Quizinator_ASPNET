using QuizinatorCore.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizinatorCore.Interfaces
{
    public interface IDatabaseService<T>
    {
        public Task<IEnumerable<T>> GetAll();
        public void Add(T item);
        public void AddMultiple(T[] newItems);
        public void Replace(T updatedItem);
        public void Delete(Guid id);
        public void AddRating(Guid itemId, int rating);
    }
}