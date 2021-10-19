using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DietBook.MVC.Models
{
    public class Recipe
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [Required]
        [MinLength(2)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [JsonPropertyName("author")]
        public string Author { get; set; }

        [Display(Name = "Created on")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("steps")]
        public List<string> Steps { get; set; }

        [JsonPropertyName("recipeIngredients")]
        public List<Ingredient> RecipeIngredients { get; set; }

        [JsonPropertyName("photo")]
        public string Photo { get; set; }
    }
}
