﻿using System.Text.Json.Serialization;

namespace Deputados.Models
{
    public class Links
    {
        [JsonPropertyName("rel")]
        public string Rel { get; set; }
        
        [JsonPropertyName("href")]
        public string Href { get; set; }

    }
}