using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DietBook.MVC.Models
{
    public class Unit
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [Required]
        [MinLength(2)]
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
