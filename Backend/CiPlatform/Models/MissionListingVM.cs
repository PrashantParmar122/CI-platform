using CiPlatform.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CiPlatform.Models
{
    public class MissionListingVM
    {
        public ListOfObject<MissionCard> missions { get; set; }
        public List<string> selectedskill { get; set; }
        public List<string> selectedtheme { get; set; }
        public List<string> selectedcity { get; set; }
        public List<string> selectedcountry { get; set; }
        public IEnumerable<SelectListItem> CityList { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> ThemeList { get; set; }
        public IEnumerable<SelectListItem> SkillList { get; set; }
    }
}
