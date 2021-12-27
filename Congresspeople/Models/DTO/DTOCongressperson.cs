using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Congressperson.Models.DTO
{
    public class DTOCongressperson
    {
        [JsonPropertyName("dados")]
        public IEnumerable<Congressperson> Congressperson { get; set; }

        [JsonPropertyName("links")]
        public IEnumerable<Links> Links { get; set; }
    }
}
