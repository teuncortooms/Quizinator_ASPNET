using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuizinatorCore.Entities.Idioms
{
    public class Idiom
    {
        public Guid IdiomId { get; private set; }
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

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"{IdiomId, -40}")
                .Append($"{Word,-15}")
                .Append($"{Translation,-15}")
                .Append($"{Unit,-10}")
                .Append($"{Sentence}");
            return builder.ToString();
        }
    }
}
