using CiPlatform.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CiPlatform.Models
{
    public class LostPageViewModel
    {
        [Required , EmailAddress , Display(Name ="Registered Email Address")]
        public string Email { get; set; }
        public bool Emailsent { get; set; }
        public List<Banner> banner { get; set; } = null!;
    }
}
