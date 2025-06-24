using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RecipeSharingPlatform.GCommon.ValidationConstants.Category;

namespace RecipeSharingPlatform.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength , MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;
        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
