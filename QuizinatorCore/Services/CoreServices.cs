using AutoMapper;
using QuizinatorCore.Entities.Idioms;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizinatorCore.Services
{
    public class CoreServices
    {
        public MapperConfiguration MapperConfig { get; set; }
        public FileConverter FileConverter { get; set; }

        public CoreServices()
        {
            this.MapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<Idiom, IdiomInCollection>());
            this.FileConverter = new FileConverter();
        }
    }
}
