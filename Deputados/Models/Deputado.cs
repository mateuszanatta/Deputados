using Deputados.Models.DTO;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Deputados.Models
{
    public class Deputado
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdDatabase { get; set; }

        [JsonPropertyName("id")]
        public int IdDeputado { get; set; }

        [JsonPropertyName("uri")]
        public string URI { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("siglaPartido")]
        public string SiglaPartido { get; set; }

        [JsonPropertyName("uriPartido")]
        public string URIPartido { get; set; }

        [JsonPropertyName("siglaUf")]
        public string UF { get; set; }

        [JsonPropertyName("idLegislatura")]
        public int IdLegislatura { get; set; }

        [JsonPropertyName("urlFoto")]
        public string URLFoto { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        public IEnumerable<Expenses> Expenses { get; set; }
    }
}
