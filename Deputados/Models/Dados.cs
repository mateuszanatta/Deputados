using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Deputados.Models
{
    public class Dados
    {
        [JsonPropertyName("dados")]
        public IEnumerable<Deputado> Deputados { get; set; }

        [JsonPropertyName("links")]
        public IEnumerable<Links> Links { get; set; }
    }
}
