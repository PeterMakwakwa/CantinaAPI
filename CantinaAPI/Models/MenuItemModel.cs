using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CantinaAPI.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class MenuItemModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Required]
        public string ImageUrl { get; set; } = string.Empty;    
        public string Type { get; set; } = string.Empty; // "Dish" or "Drink"
        public ICollection<ReviewModel> Reviews { get; set; }= new List<ReviewModel>();
    }
}
