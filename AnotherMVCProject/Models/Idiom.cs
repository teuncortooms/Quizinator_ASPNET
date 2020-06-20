using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnotherMVCProject.Models
{
    public class Idiom
    {
        public Guid IdiomId { get; set; }
        public string Word { get; set; }
        [JsonPropertyName("exampleSentence")]
        public string Sentence { get; set; }
        public string Translation { get; set; }
        public string Unit { get; set; }
        public int[] Ratings { get; set; }
    }
}
