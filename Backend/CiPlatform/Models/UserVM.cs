using CiPlatform.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace CiPlatform.Models
{
    public class UserVM
    {
        public User User { get; set; }
        public IEnumerable<SelectListItem> CityList { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> SkillList { get; set; }
        public List<String> Skillname { get; set; }
    }
}
