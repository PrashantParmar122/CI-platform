using CiPlatform.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CiPlatform.Models
{
    public class ResetPasswordVM
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        
        [Required]
        public string Password { get; set; } = null!;
        public List<Banner> banner { get; set; } = null!;
    }
}
