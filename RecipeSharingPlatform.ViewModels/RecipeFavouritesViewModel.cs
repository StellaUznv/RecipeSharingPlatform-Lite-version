using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSharingPlatform.ViewModels
{
    public class RecipeFavouritesViewModel
    {
        public int RecipeId { get; set; }
        public string Title { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string Category { get; set; } = null!;
    }

}
