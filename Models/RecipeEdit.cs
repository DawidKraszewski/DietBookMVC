using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DietBook.MVC.Models
{
    public class RecipeEdit : Recipe
    {
        [JsonPropertyName("newPhoto")]
        public IFormFile NewPhoto { get; set; }
    }
}
