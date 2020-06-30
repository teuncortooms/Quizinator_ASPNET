using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuizinatorCore.Entities
{
    public class Idiom
    {
        public Guid IdiomId { get; set; }
        [Required]
        public string Word { get; set; }
        [Required]
        [JsonPropertyName("exampleSentence")]
        public string Sentence { get; set; }
        [Required]
        public string Translation { get; set; }
        [Required]
        public string Unit { get; set; }
        public int[] Ratings { get; set; }
    }
}
