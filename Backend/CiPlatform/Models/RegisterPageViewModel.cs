using CiPlatform.DataModels;

namespace CiPlatform.Models
{
    public class RegisterPageViewModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int PhoneNumber { get; set; }

        public List<Banner> banner { get; set; } = null!;
    }
}
