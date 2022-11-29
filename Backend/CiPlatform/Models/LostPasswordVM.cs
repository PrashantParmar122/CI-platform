using CiPlatform.DataModels;
using System.Xml;

namespace CiPlatform.Models
{
    public class LostPasswordVM
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public List<Banner> banner { get; set; } = null!;
    }
}
