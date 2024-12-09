using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CantinaAPI.Models
{
    public class UserModel: IdentityUser
    {
        [Required, MaxLength(60)]
        public string FullName { get; set; } = string.Empty;
    }
}
