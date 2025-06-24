using RecipeSharingPlatform.Data.Models;
using RecipeSharingPlatform.ViewModels;

namespace RecipeSharingPlatform.Services.Core.Contracts
{
    public interface IRecipeService
    {
        public Task<IEnumerable<RecipeViewModel>> GetRecipesAsync(string? userId);
        public Task<RecipeCreateViewModel> GetCreateModelAsync();
        public Task CreateRecipeAsync(RecipeCreateViewModel model, string userId);
        public Task AddToFavouritesAsync(int recipeId, string userId);
        public Task<IEnumerable<RecipeFavouritesViewModel>> GetUserFavouritesAsync(string userId);
        public Task RemoveFromFavourites(int recipeId, string userId);
        public Task<RecipeDetailsViewModel> GetRecipeDetailsAsync(int recipeId, string userId);
        public Task EditRecipeAsync(RecipeEditViewModel model);
        public Task<Recipe> GetRecipeByIdAsync(int recipeId);
        public Task<RecipeEditViewModel> GetEditModelAsync();
        public Task DeleteRecipeAsync(int recipeId);
    }
}
