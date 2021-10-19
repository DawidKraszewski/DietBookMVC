using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DietBook.MVC.Models
{
    public class Ingredient
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [Required]
        [MinLength(2)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("unit")]
        public Unit Unit { get; set; }

        [JsonPropertyName("quantity")]
        public float? Quantity { get; set; }

        [JsonPropertyName("ingredientRecipes")]
        public List<Recipe> IngredientRecipes { get; set; }
    }
}
