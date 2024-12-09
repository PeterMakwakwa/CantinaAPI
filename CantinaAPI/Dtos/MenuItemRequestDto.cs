using CantinaAPI.Enums;
using CantinaAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CantinaAPI.Dtos
{
    public class MenuItemRequestDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Required, Url]
        public string ImageUrl { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
