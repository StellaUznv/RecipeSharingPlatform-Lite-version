using Microsoft.EntityFrameworkCore;
using RecipeSharingPlatform.Data;
using RecipeSharingPlatform.Data.Models;
using RecipeSharingPlatform.Services.Core.Contracts;
using RecipeSharingPlatform.ViewModels;
using System.Security.Claims;

namespace RecipeSharingPlatform.Services.Core
{
    public class RecipeService : IRecipeService
    {
        private readonly ApplicationDbContext _context;
        public RecipeService(ApplicationDbContext dbContext)
        {
            this._context = dbContext;
        }
        public async Task<IEnumerable<RecipeViewModel>> GetRecipesAsync(string? userId)
        {
            var recipes = await _context.Recipes
                .Where(r => !r.IsDeleted)
                .Include(r=>r.Category)
                .Include(r=>r.UsersRecipes)
                .ToListAsync();

            var result = recipes.Select(r => new RecipeViewModel
            {
                Id = r.Id,
                Category = r.Category.Name,
                ImageUrl = r.ImageUrl,
                IsAuthor = userId != null && r.AuthorId == userId,
                IsSaved = userId != null && r.UsersRecipes.Any(ur => ur.UserId == userId),
                SavedCount = r.UsersRecipes.Count,
                Title = r.Title
            }).ToList();

            return result;
        }
        public async Task<RecipeCreateViewModel> GetCreateModelAsync()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .ToListAsync();

            return new RecipeCreateViewModel
            {
                CreatedOn = DateTime.Today,
                Categories = categories
            };
        }

        public async Task CreateRecipeAsync(RecipeCreateViewModel model, string userId)
        {
            var category = await _context.Categories.FindAsync(model.CategoryId);
            var recipe = new Recipe
            {
                Author =_context.Users.Find(userId),
                AuthorId = userId,
                Category = category,
                CategoryId = model.CategoryId,
                CreatedOn = model.CreatedOn,
                ImageUrl = model.ImageUrl,
                Instructions = model.Instructions,
                Title = model.Title
            };

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task AddToFavouritesAsync(int recipeId, string userId)
        {
            var exists = await _context.UserRecipes
                .AnyAsync(ur => ur.RecipeId == recipeId && ur.UserId == userId);

            if (!exists)
            {
                var userRecipe = new UserRecipe
                {
                    RecipeId = recipeId,
                    UserId = userId
                };

                await _context.UserRecipes.AddAsync(userRecipe);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<RecipeFavouritesViewModel>> GetUserFavouritesAsync(string userId)
        {
            return await _context.UserRecipes.Where(ur => ur.UserId == userId)
                .Select(ur => new RecipeFavouritesViewModel
                {
                    Category = ur.Recipe.Category.Name,
                    ImageUrl = ur.Recipe.ImageUrl,
                    RecipeId = ur.RecipeId,
                    Title = ur.Recipe.Title
                }).ToListAsync();
        }

        public async Task RemoveFromFavourites(int recipeId, string userId)
        {
            var item = await _context.UserRecipes
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RecipeId == recipeId);
            if (item != null)
            {
                _context.UserRecipes.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<RecipeDetailsViewModel> GetRecipeDetailsAsync(int recipeId, string userId)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.UsersRecipes)
                .Include(r=>r.Author)
                .FirstOrDefaultAsync(r => r.Id == recipeId && !r.IsDeleted);

            if (recipe == null)
            {
                throw new ArgumentException("Recipe not found!");
            }

            return new RecipeDetailsViewModel
            {
                Id = recipeId,
                Author = recipe.Author.Email,
                Category = recipe.Category.Name,
                CreatedOn = recipe.CreatedOn,
                ImageUrl = recipe.ImageUrl,
                Instructions = recipe.Instructions,
                IsAuthor = recipe.AuthorId == userId,
                IsSaved = recipe.UsersRecipes.Any(ur=>ur.UserId == userId),
                Title = recipe.Title
            };
        }

        public async Task<Recipe> GetRecipeByIdAsync(int id)
        {
            var recipe = await _context.Recipes
                .Include(r=>r.Author)
                .FirstOrDefaultAsync(r=>r.Id == id && !r.IsDeleted);
            if (recipe == null)
            {
                throw new ArgumentException("Recipe not found!");
            }

            return recipe;
        }

        public async Task<RecipeEditViewModel> GetEditModelAsync()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .ToListAsync();

            return new RecipeEditViewModel
            {
                Categories = categories
            };
        }

        public async Task EditRecipeAsync(RecipeEditViewModel model)
        {
            var recipe = await GetRecipeByIdAsync(model.Id);
            recipe.Title = model.Title;
            recipe.CategoryId = model.CategoryId;
            recipe.Instructions = model.Instructions;
            recipe.ImageUrl = model.ImageUrl;
            recipe.CreatedOn = model.CreatedOn;

            await _context.SaveChangesAsync();

        }

        public async Task DeleteRecipeAsync(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                throw new ArgumentException("Recipe not found!");
            }

            recipe.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
