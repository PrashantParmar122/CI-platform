using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CiPlatform.Models
{
    public class UpsertStoryVM
    {
        public IEnumerable<SelectListItem> MissionList { get; set; }
        [Required]
        public long MissionId { get; set; }
        public string Title { get; set; } = null!;
        [Required]
        public DateTime date { get; set; }
        public string VideoURL { get; set; }
        public string Description { get; set; } = null!;
    }
}
