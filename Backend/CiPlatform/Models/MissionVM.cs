using CiPlatform.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CiPlatform.Models
{
    public class MissionVM
    {
        public Mission Mission { get; set; }
        public GoalMission goalMission { get; set; }
        public List<string> addskill { get; set; }
        public IEnumerable<SelectListItem> CityList { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> ThemeList { get; set; }
        public IEnumerable<SelectListItem> SkillList { get; set; }
    }
}
