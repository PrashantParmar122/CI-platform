using CiPlatform.DataModels;

namespace CiPlatform.Models
{
    public class LoginPageViewModel
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public List<Banner> banner { get; set; } = null!;
    }
}
