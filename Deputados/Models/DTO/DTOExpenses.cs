using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Deputados.Models.DTO
{
    public class Expenses
    {
        [JsonPropertyName("ano")]
        public int Ano { get; set; }

        [JsonPropertyName("mes")]
        public int Mes { get; set; }

        [JsonPropertyName("tipoDespesa")]
        public string TipoDespesa { get; set; }

        [JsonPropertyName("tipoDocumento")]
        public string TipoDocumento { get; set; }

        [JsonPropertyName("valorDocumento")]
        public decimal ValorDocumento { get; set; }
        
        [JsonPropertyName("valorLiquido")]
        public decimal valorLiquido { get; set; }

        [JsonPropertyName("valorGlosa")]
        public decimal ValorGlosa { get; set; }
    }

    public class DTOExpenses
    {
        [JsonPropertyName("dados")]
        public ICollection<Expenses> Expenses { get; set; }
        [JsonPropertyName("links")]
        public ICollection<Links> Links { get; set; }
    }
}
