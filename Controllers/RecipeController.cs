using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DietBook.MVC.Data;
using DietBook.MVC.Models;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using System.Net.Mime;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using AutoMapper;

namespace DietBook.MVC.Controllers
{
    public class RecipeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper mapper;
        private readonly string ApiUrl = "http://dietbookapi.germanywestcentral.azurecontainer.io/api/recipe/";

        public RecipeController(IHttpClientFactory clientFactory, IMapper mapper)
        {
            _clientFactory = clientFactory;
            this.mapper = mapper;
        }

        // GET: Recipe
        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            var streamTask = client.GetStreamAsync(ApiUrl);
            var recipes = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Recipe>>(await streamTask);

            if (TempData.ContainsKey("addedIngredients"))
            {
                var test = JsonConvert.DeserializeObject<List<Ingredient>>(TempData["addedIngredients"].ToString());
            }

            return View(recipes);

        }

        // GET: Recipe/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _clientFactory.CreateClient();
            var recipe = await client.GetFromJsonAsync(ApiUrl + "ingredients/" + id, typeof(Recipe));

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipe/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] RecipeCreate recipe)
        {
            if (ModelState.IsValid)
            {
                recipe.Id = this.ResolveId(recipe);

                if (TempData.ContainsKey("addedIngredients"))
                {
                    recipe.RecipeIngredients = JsonConvert.DeserializeObject<List<Ingredient>>(TempData["addedIngredients"].ToString());
                }

                var content = new MultipartFormDataContent();

                var jsonCont = JsonContent.Create<List<Ingredient>>(recipe.RecipeIngredients);

                content.Add(jsonCont, "RecipeIngredients");

                foreach (var key in Request.Form.Keys)
                {
                    var stringCont = new StringContent(Request.Form[key]);
                    content.Add(stringCont, key);
                }

                var file = Request.Form.Files.FirstOrDefault();
                if (file != null)
                {
                    content.Add(new StreamContent(file.OpenReadStream()), "Photo", file.FileName);
                }

                var client = _clientFactory.CreateClient();
                var result = await client.PostAsync(ApiUrl, content);

                var createdRecipe = Newtonsoft.Json.JsonConvert.DeserializeObject<Recipe>(await result.Content.ReadAsStringAsync());

                return View(nameof(Details), createdRecipe);
            }

            return View();
        }

        // GET: Recipe/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _clientFactory.CreateClient();
            var recipe = await client.GetFromJsonAsync(ApiUrl + "ingredients/" + id, typeof(Recipe));

            if (recipe == null)
            {
                return NotFound();
            }

            var steps = string.Join(Environment.NewLine + "" + Environment.NewLine, (recipe as Recipe).Steps);
            (recipe as Recipe).Steps.Clear();
            (recipe as Recipe).Steps.Add(steps);


            TempData["editedIngredients"] = JsonConvert.SerializeObject((recipe as Recipe).RecipeIngredients);

            var recipeEdit = this.mapper.Map<Recipe, RecipeEdit>(recipe as Recipe);

            return View(recipeEdit);
        }

        // POST: Recipe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [FromForm] RecipeEdit recipe)
        {
            if (Guid.Parse(id) != recipe.Id)
            {
                return NotFound();
            }

            if (TempData.ContainsKey("addedIngredients"))
            {
                recipe.RecipeIngredients = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Ingredient>>(TempData["addedIngredients"].ToString());
            }

            var content = new MultipartFormDataContent();

            var jsonCont = JsonContent.Create<List<Ingredient>>(recipe.RecipeIngredients);

            content.Add(jsonCont, "RecipeIngredients");

            foreach (var key in Request.Form.Keys)
            {
                var stringCont = new StringContent(Request.Form[key]);
                content.Add(stringCont, key);
            }

            var file = Request.Form.Files.FirstOrDefault();
            if (file != null)
            {
                content.Add(new StreamContent(file.OpenReadStream()), "NewPhoto", file.FileName);
            }

            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient();
                var streamTask = await client.PutAsync(ApiUrl, content);

                return RedirectToAction(nameof(Index));
            }

            return View(recipe);
        }

        // GET: Recipe/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (Guid.Parse(id) == Guid.Empty)
            {
                return NotFound();
            }

            var client = _clientFactory.CreateClient();
            var recipe = await client.GetFromJsonAsync(ApiUrl + id, typeof(Recipe));

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipe/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var client = _clientFactory.CreateClient();
            var streamTask = await client.DeleteAsync(ApiUrl + id);

            return RedirectToAction(nameof(Index));
        }

        protected virtual Guid ResolveId(RecipeCreate entity) => entity.Id == Guid.Empty ? Guid.NewGuid() : entity.Id;

        protected virtual List<Ingredient> AddUsedIngredient(List<Ingredient> usedIngredients, Ingredient ingredient)
        {
            usedIngredients.Add(ingredient);

            return usedIngredients;
        }
            
    }
}
