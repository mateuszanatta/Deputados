using Congressperson.Models.DTO;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Congressperson.Models
{
    public class Congressperson
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdDatabase { get; set; }

        [JsonPropertyName("id")]
        public int IdCongressperson { get; set; }

        [JsonPropertyName("uri")]
        public string URI { get; set; }

        [JsonPropertyName("nome")]
        public string Name { get; set; }

        [JsonPropertyName("siglaPartido")]
        public string PartyAcronym { get; set; }

        [JsonPropertyName("uriPartido")]
        public string URIParty { get; set; }

        [JsonPropertyName("siglaUf")]
        public string State { get; set; }

        [JsonPropertyName("idLegislatura")]
        public int IdLegislature { get; set; }

        [JsonPropertyName("urlFoto")]
        public string PictureURL { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        public IEnumerable<Expenses> Expenses { get; set; }
    }
}
