using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CiPlatform.Models
{
    public class TimeSheetVM
    {
        public IEnumerable<SelectListItem> MissionList { get; set; }
        public long Timesheetid { get; set; }
        [Required]
        public long? Missionid { get; set; }
        public int Missiontype { get; set; }
        public string MissionTitle { get; set; }
        [Required]
        public DateTime date { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Only positive number allowed.")]
        public int? task { get; set; }
        [Required]
        [Range (0,12)]
        public int? hour { get; set; }
        [Required]
        [Range(0,60)]
        public int? min { get; set; }
        public string Notes { get; set; } = null!;
    }
}
