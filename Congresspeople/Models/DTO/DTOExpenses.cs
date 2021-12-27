using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Congressperson.Models.DTO
{
    public class Expenses
    {
        [JsonPropertyName("ano")]
        public int Year { get; set; }

        [JsonPropertyName("mes")]
        public int Month { get; set; }

        [JsonPropertyName("tipoDespesa")]
        public string ExpenseType { get; set; }

        [JsonPropertyName("tipoDocumento")]
        public string TypeDocument { get; set; }

        [JsonPropertyName("valorDocumento")]
        public decimal GrossAmount { get; set; }
        
        [JsonPropertyName("valorLiquido")]
        public decimal NetAmount { get; set; }

        [JsonPropertyName("valorGlosa")]
        public decimal NonRefundableAmount { get; set; }
    }

    public class DTOExpenses
    {
        [JsonPropertyName("dados")]
        public ICollection<Expenses> Expenses { get; set; }
        [JsonPropertyName("links")]
        public ICollection<Links> Links { get; set; }
    }
}
