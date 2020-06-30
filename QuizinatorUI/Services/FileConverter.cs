using QuizinatorUI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuizinatorUI.Services
{
    public class FileConverter
    {
        string contents;

        public T[] ConvertFileToObjects<T>(IFormFile file)
        { 
            using (StreamReader fileReader = new StreamReader(file.OpenReadStream()))
            {
                this.contents = fileReader.ReadToEnd();
            }

            string contentType = file.ContentType;
            return contentType switch
            {
                "application/json" => ConvertJsonToObjects<T>(this.contents),
                _ => throw new FormatException(),
            };
        }

        public T[] ConvertJsonToObjects<T>(string json)
        {
            return JsonSerializer.Deserialize<T[]>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
