using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Migrations;
using RecipeSharingPlatform.Services.Core.Contracts;
using RecipeSharingPlatform.ViewModels;

namespace RecipeSharingPlatform.Web.Controllers
{
    public class RecipeController : Controller
    {
        private readonly IRecipeService _service;
        public RecipeController(IRecipeService service)
        {
            this._service = service;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var recipes = await _service.GetRecipesAsync(userId);

            return View(recipes);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _service.GetCreateModelAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecipeCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await _service.CreateRecipeAsync(model, userId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Save(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _service.AddToFavouritesAsync(id, userId);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _service.GetUserFavouritesAsync(userId));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _service.RemoveFromFavourites(id, userId);
            return RedirectToAction(nameof(Favorites));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.Identity.IsAuthenticated
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)!
                : string.Empty;

            return View(await _service.GetRecipeDetailsAsync(id, userId));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var recipe = await _service.GetRecipeByIdAsync(id);
            var model = await _service.GetEditModelAsync();

            var result = new RecipeEditViewModel
            {
                ImageUrl = recipe.ImageUrl,
                Categories = model.Categories,
                CategoryId = recipe.CategoryId,
                CreatedOn = recipe.CreatedOn,
                Id = recipe.Id,
                Instructions = recipe.Instructions,
                Title = recipe.Title
            };
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RecipeEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _service.EditRecipeAsync(model);
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var recipe = await _service.GetRecipeByIdAsync(id);
            var model = new RecipeDeleteViewModel
            {
                Id = recipe.Id,
                Author = recipe.Author.Email,
                AuthorId = recipe.AuthorId,
                Title = recipe.Title
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            await _service.DeleteRecipeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
