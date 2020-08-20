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

namespace QuizinatorInfrastructure.Services
{
    public abstract class JsonFileService<T> : IDatabaseService<T>
    {
        protected string JsonFileName;

        protected abstract Guid GetId(T item);
        protected abstract T AddIdToNewItem(T newItem);
        public abstract Task AddRatingAsync(Guid itemId, int rating);

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            using FileStream fs = File.OpenRead(JsonFileName);
            return await JsonSerializer.DeserializeAsync<IEnumerable<T>>(fs, options);
        }

        public async Task AddAsync(T item)
        {
            IEnumerable<T> items = await GetAllAsync();
            items = AddIdToNewItemAndAppend(item, items);
            await UpdateSourceAsync(items);
        }

        public async Task AddMultipleAsync(T[] newItems)
        {
            IEnumerable<T> items = await GetAllAsync();
            foreach (T item in newItems)
            {
                items = AddIdToNewItemAndAppend(item, items);
            }
            await UpdateSourceAsync(items);
        }

        protected IEnumerable<T> AddIdToNewItemAndAppend(T item, IEnumerable<T> items)
        {
            T newItem = AddIdToNewItem(item);
            items = items.Append(newItem);
            return items;
        }

        public async Task ReplaceAsync(T updatedItem)
        {
            List<T> items = (await GetAllAsync()).ToList();
            int index = items.FindIndex(x => GetId(x) == GetId(updatedItem));
            items[index] = updatedItem;
            await UpdateSourceAsync(items);
        }

        public async Task DeleteAsync(Guid id)
        {
            List<T> idioms = (await GetAllAsync()).ToList();
            idioms.RemoveAll(x => GetId(x) == id);
            await UpdateSourceAsync(idioms);
        }

        protected async Task UpdateSourceAsync(IEnumerable<T> items)
        {
            using FileStream fs = File.Open(JsonFileName, FileMode.Create);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            await JsonSerializer.SerializeAsync(fs, items, options);
        }
    }
}



