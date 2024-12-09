using System.ComponentModel.DataAnnotations;

namespace CantinaAPI.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public UserModel? User { get; set; }
        public int ItemId { get; set; }
        public MenuItemModel? Item { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        [MaxLength(500)]
        public string Comment { get; set; } = string.Empty;
    }
}
