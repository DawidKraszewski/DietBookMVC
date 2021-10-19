using DietBook.MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace DietBook.MVC.Controllers
{
    public class IngredientController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string ApiUrl = "http://dietbookapi.germanywestcentral.azurecontainer.io/api/ingredient/";
        private readonly string UnitApiUrl = "http://dietbookapi.germanywestcentral.azurecontainer.io/api/unit/";

        public IngredientController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }


        // GET: Ingredient
        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            var streamTask = await client.GetStreamAsync(ApiUrl);
            IEnumerable<Ingredient>ingredients = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Ingredient>>(streamTask);

            return View(ingredients.OrderBy(i => i.Name));
        }

        // GET: Recipe/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _clientFactory.CreateClient();
            var recipe = await client.GetFromJsonAsync(ApiUrl + "recipes/" + id, typeof(Ingredient));

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
        public async Task<IActionResult> Create([Bind("Name")] IngredientCreate ingredient)
        {
            if (ModelState.IsValid)
            {
                ingredient.Id = this.ResolveId(ingredient);

                var client = _clientFactory.CreateClient();
                var streamTask = await client.PostAsJsonAsync(ApiUrl, ingredient);

                return RedirectToAction(nameof(Index));
            }

            return View(ingredient);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (Guid.Parse(id) == Guid.Empty)
            {
                return NotFound();
            }

            var client = _clientFactory.CreateClient();
            var recipe = await client.GetFromJsonAsync(ApiUrl + "recipes/" + id, typeof(Ingredient));

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


        public async Task<IActionResult> SearchForIngredientsIndex()
        {
            var client = _clientFactory.CreateClient();
            var streamTask = await client.GetStreamAsync(ApiUrl);
            var ingredients = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Ingredient>>(streamTask);

            TempData["allIngredientsList"] = JsonConvert.SerializeObject(ingredients.OrderBy(i=>i.Name));

            return PartialView("_SearchForIngredients", ingredients.OrderBy(i=>i.Name));

        }


        public IActionResult SearchForIngredients(IngredientSearchObject obj)
        {
            if (obj.searchString == null)
                return RedirectToAction("SearchForIngredientsIndex");

            var inputList = JsonConvert.DeserializeObject<List<Ingredient>>(TempData.Peek("allIngredientsList").ToString());

            var ingredients = inputList.AsQueryable().Where(x => x.Name.Contains(obj.searchString)).OrderBy(i=>i.Name).ToList();

            if (ingredients == null)
            {
                return NotFound();
            }

            return PartialView("_SearchForIngredients", ingredients);
        }

        public IActionResult GetIngredientsForRecipe()
        {
            return PartialView("_SearchForIngredientsForRecipe");
        }

        public async Task<IActionResult> SearchForIngredientsForRecipeIndex()
        {
            var client = _clientFactory.CreateClient();
            var streamTask = await client.GetStreamAsync(ApiUrl);
            var ingredients = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Ingredient>>(streamTask);

            var unitStreamTask = await client.GetStreamAsync(UnitApiUrl);
            var units = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Unit>>(unitStreamTask);


            TempData["allUnits"] = JsonConvert.SerializeObject(units);
            TempData["allIngredientsList"] = JsonConvert.SerializeObject(ingredients);

            return PartialView("_SearchingForIngredientsResult", ingredients.OrderBy(i=>i.Name));

        }

        public IActionResult SearchForIngredientsForRecipe(IngredientSearchObject obj)
        {
            if (obj.searchString == null)
                return RedirectToAction("SearchForIngredientsForRecipeIndex");

            var inputList = JsonConvert.DeserializeObject<List<Ingredient>>(TempData.Peek("allIngredientsList").ToString());

            var ingredients = inputList.AsQueryable().Where(x => x.Name.Contains(obj.searchString)).ToList();

            if (ingredients == null)
            {
                return NotFound();
            }

            return PartialView("_SearchingForIngredientsResult", ingredients.OrderBy(i=>i.Name));
        }

        public IActionResult AddIngredient(Ingredient ingredient)
        {
            var addedIngredients = new List<Ingredient>();

            if (TempData.ContainsKey("addedIngredients"))
            {
                addedIngredients = JsonConvert.DeserializeObject<List<Ingredient>>(TempData.Peek("addedIngredients").ToString());
            }

            if (!addedIngredients.Exists(ai=>ai.Id == ingredient.Id))
            {
                addedIngredients.Add(ingredient);
            }

            TempData["addedIngredients"] = JsonConvert.SerializeObject(addedIngredients);

            return PartialView("_AddedIngredients", addedIngredients);
        }

        public async Task<IActionResult> AddIngredientFromInput(IngredientCreate ingredient)
        {
            ingredient.Id = this.ResolveId(ingredient);

            var client = _clientFactory.CreateClient();
            var result = await client.PostAsJsonAsync(ApiUrl, ingredient);

            var secondClient = _clientFactory.CreateClient();
            var streamTask = await secondClient.GetStreamAsync(ApiUrl);
            var ingredients = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Ingredient>>(streamTask);

            TempData.Remove("allIngredientsList");
            TempData["allIngredientsList"] = JsonConvert.SerializeObject(ingredients);

            return PartialView("_SearchingForIngredientsResult", ingredients.OrderBy(i=>i.Name));  
        }

        public IActionResult DeleteIngredient(Ingredient ingredient)
        {
            var addedIngredients = new List<Ingredient>();

            if (TempData.ContainsKey("addedIngredients"))
            {
                addedIngredients = JsonConvert.DeserializeObject<List<Ingredient>>(TempData.Peek("addedIngredients").ToString());
            }

            var ingredientToRemove = addedIngredients.FirstOrDefault(a =>a.Id == ingredient.Id);

            var result = addedIngredients.Remove(ingredientToRemove);

            TempData["addedIngredients"] = JsonConvert.SerializeObject(addedIngredients);

            return PartialView("_AddedIngredients", addedIngredients);
        }

        public IActionResult GetAddedIngredients()
        {
            var addedIngredients = new List<Ingredient>();

            if (TempData.ContainsKey("addedIngredients"))
            {
                addedIngredients = JsonConvert.DeserializeObject<List<Ingredient>>(TempData.Peek("addedIngredients").ToString());
            }

            else if (TempData.ContainsKey("editedIngredients"))
            {
                TempData["addedIngredients"] = TempData["editedIngredients"];
                TempData.Remove("editedIngredients");

                addedIngredients = JsonConvert.DeserializeObject<List<Ingredient>>(TempData.Peek("addedIngredients").ToString());  
            }

            return PartialView("_AddedIngredients", addedIngredients);
        }

        public IActionResult GetIngredientsInput()
        {
            return PartialView("_IngredientsInput");
        }

        protected virtual Guid ResolveId(IngredientCreate entity) => entity.Id == Guid.Empty ? Guid.NewGuid() : entity.Id;

    }
}
