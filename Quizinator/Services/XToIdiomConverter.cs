using Quizinator.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Quizinator.Services
{
    public class XToIdiomConverter
    {
        string contents;

        public Idiom[] ConvertFileToIdioms(IFormFile file)
        { 
            using (StreamReader fileReader = new StreamReader(file.OpenReadStream()))
            {
                this.contents = fileReader.ReadToEnd();
            }

            string contentType = file.ContentType;
            return contentType switch
            {
                "application/json" => ConvertJsonToIdioms(this.contents),
                _ => throw new FormatException(),
            };
        }

        public Idiom[] ConvertJsonToIdioms(string json)
        {
            return JsonSerializer.Deserialize<Idiom[]>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }


    }
}
