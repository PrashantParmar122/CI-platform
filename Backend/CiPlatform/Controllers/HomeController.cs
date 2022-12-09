using CiPlatform.DataModels;
using CiPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace CiPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly CiPlatformContext _db = new CiPlatformContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [CheckSession]
        [HttpGet]
        public IActionResult MissionListing()
        {
            MissionListingVM missionListingVM = new MissionListingVM();
            missionListingVM.CountryList = _db.Countries.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CountryId.ToString()
            });
            missionListingVM.ThemeList = _db.MissionThemes.Select(i => new SelectListItem
            {
                Text = i.Title,
                Value = i.MissionThemeId.ToString()
            });
            missionListingVM.SkillList = _db.Skills.Select(i => new SelectListItem
            {
                Text = i.SkillName,
                Value = i.SkillId.ToString()
            });
            missionListingVM.CityList = _db.Cities.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CityId.ToString()
            });

            var missionlist = _db.Missions.Where(u => u.DeletedAt == null && u.Status == 1).Select(i => i.MissionId).ToList();
            
            List<MissionCard> temp = new List<MissionCard>();
            foreach (var item in missionlist)
            {
                var obj = GetMissionById(item);
                temp.Add(obj);
            }

            ListOfObject<MissionCard> Cardlist = new ListOfObject<MissionCard>();
            Cardlist.Records = temp;
            Cardlist.total_Records = temp.Count();
            missionListingVM.missions = Cardlist;
            return View(missionListingVM);
        }


        [HttpPost]
        public PartialViewResult GetMission(List<long> countryid, List<long> cityid, List<long> themeid, List<long> skillid, int sort, string keyword = "")
        {
            MissionCard missionCard = new MissionCard();
            List<long> list = new List<long>();

            if(countryid.Count() == 0 && cityid.Count() == 0 && themeid.Count() == 0 && skillid.Count() == 0 && keyword == null)
            {
                list = _db.Missions.Where(u => u.DeletedAt == null && u.Status == 1).Select(i => i.MissionId).ToList();
            }
            else
            {
                var missionskill = _db.MissionSkills.Where(p => p.DeletedAt == null && skillid.Contains(p.SkillId)).Select(i => i.MissionId).ToList();
                list = _db.Missions.Where(u => cityid.Contains(u.CityId) 
                                    || countryid.Contains(u.CountryId) 
                                    || themeid.Contains(u.ThemeId) 
                                    || u.Title.ToLower().Contains(keyword)
                                    || missionskill.Contains(u.MissionId))
                               .Select(i => i.MissionId)
                               .ToList();
            }            
            
            List<MissionCard> temp = new List<MissionCard>();
            foreach (var item in list)
            {
                var obj = GetMissionById(item);
                temp.Add(obj);
            }

            if (sort != 0)
            {
                switch (sort)
                {
                    case 1:
                        temp = temp.OrderByDescending(x => x.CreatedAt).ToList();
                        break;
                    case 2:
                        temp = temp.OrderBy(x => x.CreatedAt).ToList();
                        break;
                    case 3:
                        temp = temp.OrderBy(x => x.TotalSeat).ToList();
                        break;
                    case 4:
                        temp = temp.OrderByDescending(x => x.TotalSeat).ToList();
                        break;
                    case 5:
                        temp = temp.OrderByDescending(x => x.ismissionfav).ToList();
                        break;
                    case 6:
                        temp = temp.OrderByDescending(x => x.Deadline).ToList();
                        break;
                }
            }
            return PartialView("_CardPartial", temp);
        }

        [CheckSession]
        [HttpGet]
        public MissionCard GetMissionById(long id)
        {
            var item = _db.Missions.FirstOrDefault(u => u.MissionId == id);

            var obj1 = _db.MissionThemes.FirstOrDefault(u => u.MissionThemeId == item.ThemeId);
            var obj2 = _db.Countries.FirstOrDefault(u => u.CountryId == item.CountryId);
            var obj3 = _db.Cities.FirstOrDefault(u => u.CityId == item.CityId);
            var obj4 = _db.MissionMedia.FirstOrDefault(u => u.MissionId == item.MissionId);
            List<long> obj5 = _db.MissionSkills.Where(u => u.MissionId == item.MissionId && u.DeletedAt == null).Select(i => i.SkillId).ToList();
            MissionCard missionCard = new MissionCard(item, obj1, obj2, obj3, obj4, obj5);
            if (item.MissionType == 2)
            {
                var obj6 = _db.GoalMissions.FirstOrDefault(u => u.MissionId == item.MissionId && u.DeletedAt == null);
                missionCard.GoalValue = obj6.GoalValue;
                missionCard.GoalObjectiveText = obj6.GoalObjectiveText;
            }
            var userid = Int64.Parse(HttpContext.Session.GetString("UserId"));
            var fav = _db.FavouriteMissions.FirstOrDefault(u => u.MissionId == id && u.UserId == userid && u.DeletedAt == null);
            if (fav != null)
                missionCard.ismissionfav = true;
            else
                missionCard.ismissionfav = false;
            return missionCard;
        }

        [CheckSession]
        [HttpPost]
        public IActionResult addtofav(int id,bool alreadyfav)
        {
            var userid = Int64.Parse(HttpContext.Session.GetString("UserId"));
            if (alreadyfav)
            {
                // remove from fav
                var obj = _db.FavouriteMissions.FirstOrDefault(u => u.MissionId == id && u.UserId == userid && u.DeletedAt == null);
                if(obj != null)
                {
                    obj.DeletedAt = DateTime.Now;
                    _db.FavouriteMissions.Update(obj);
                    _db.SaveChanges();
                }
            }
            else
            {
                // add to fav
                FavouriteMission favouriteMission = new FavouriteMission();
                favouriteMission.MissionId = id;
                favouriteMission.UserId = userid;
                favouriteMission.CreatedAt = DateTime.Now;
                _db.Add(favouriteMission);
                _db.SaveChanges();
            }
            return Ok("Done");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}