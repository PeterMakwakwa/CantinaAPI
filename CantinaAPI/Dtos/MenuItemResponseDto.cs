using CantinaAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace CantinaAPI.Dtos
{
    public class MenuItemResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty; // "Dish" or "Drink"
       // public ICollection<ReviewModel>? Reviews { get; set; } = new List<ReviewModel>();
    }
}
