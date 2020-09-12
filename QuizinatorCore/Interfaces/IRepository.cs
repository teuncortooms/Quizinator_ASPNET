using QuizinatorCore.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizinatorCore.Interfaces
{
    public interface IRepository<T>
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task AddAsync(T item);
        public Task AddMultipleAsync(T[] newItems);
        public Task ReplaceAsync(T updatedItem);
        public Task DeleteAsync(Guid id);
        public Task AddRatingAsync(Guid itemId, int rating);
    }
}