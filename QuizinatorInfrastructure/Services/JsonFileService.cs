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
        protected readonly string JsonFileName;

        protected abstract Guid GetId(T item);
        protected abstract T AddIdToItem(T newItem);
        public abstract void AddRating(Guid itemId, int rating);

        public async Task<IEnumerable<T>> GetAll()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            using FileStream fs = File.OpenRead(JsonFileName);
            return await JsonSerializer.DeserializeAsync<IEnumerable<T>>(fs, options);
        }

        public async void Add(T item)
        {
            IEnumerable<T> items = await GetAll();
            items = items.Append(item);
            UpdateSource(items);
        }

        public async void AddMultiple(T[] newItems)
        {
            IEnumerable<T> items = await GetAll();
            foreach (T item in newItems)
            {
                T newItem = AddIdToItem(item);
                items = items.Append(newItem);
            }
            UpdateSource(items);
        }

        public async void Replace(T updatedItem)
        {
            List<T> items = (await GetAll()).ToList();
            int index = items.FindIndex(x => GetId(x) == GetId(updatedItem));
            items[index] = updatedItem;
            UpdateSource(items);
        }

        public async void Delete(Guid id)
        {
            List<T> idioms = (await GetAll()).ToList();
            idioms.RemoveAll(x => GetId(x) == id);
            UpdateSource(idioms);
        }

        protected async void UpdateSource(IEnumerable<T> items)
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



