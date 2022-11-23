using CiPlatform.DataModels;

namespace CiPlatform.Models
{
    public class ResetPageViewModel
    {
        public string Email { get; set; } = null!;

        public string Token { get; set; } = null!;

        public string Password { get; set; } = null!;

        public List<Banner> banner { get; set; } = null!;
    }
}
