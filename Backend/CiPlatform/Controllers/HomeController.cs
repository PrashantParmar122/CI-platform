using CiPlatform.DataModels;
using CiPlatform.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit.Text;
using MimeKit;
using System.Diagnostics;
using MailKit.Net.Smtp;

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
        public IActionResult MissionListing(int pageIndex = 1)
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
        public PartialViewResult GetMission(List<long> countryid, List<long> cityid, List<long> themeid, List<long> skillid, int sort,int Pageindex = 1, string keyword = "")
        {
            MissionCard missionCard = new MissionCard();
            List<long> list = new List<long>();

            if (countryid.Count() == 0 && cityid.Count() == 0 && themeid.Count() == 0 && skillid.Count() == 0 && keyword == null)
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
                        temp = temp.OrderBy(x => x.Seatleft).ToList();
                        break;
                    case 4:
                        temp = temp.OrderByDescending(x => x.Seatleft).ToList();
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
            
            // Create Mission Card
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

            var missionapplication = _db.MissionApplications.FirstOrDefault(u => u.MissionId == id && u.UserId == userid && u.DeletedAt == null);
            if (missionapplication != null)
                missionCard.ismissionapplied = true;
            else
                missionCard.ismissionapplied = false;
            missionCard.Seatleft = item.TotalSeat - _db.MissionApplications.Where(u => u.MissionId == id && u.DeletedAt == null).ToList().Count();
            
            var StarRatingList = _db.MissionRatings.Where(u => u.MissionId == id).Select(p => p.Rating).ToList();
            float totalRate = 0;
            foreach(var rating in StarRatingList)
            {
                totalRate = totalRate + (float)rating;
            }
            missionCard.avgRate = (float)totalRate/StarRatingList.Count();
            
            
            return missionCard;
        }

        [CheckSession]
        [HttpPost]
        public IActionResult addtofav(int id, bool alreadyfav)
        {
            var userid = Int64.Parse(HttpContext.Session.GetString("UserId"));
            if (alreadyfav)
            {
                // remove from fav
                var obj = _db.FavouriteMissions.FirstOrDefault(u => u.MissionId == id && u.UserId == userid && u.DeletedAt == null);
                if (obj != null)
                {
                    obj.DeletedAt = DateTime.Now;
                    _db.FavouriteMissions.Update(obj);
                    _db.SaveChanges();
                    // TempData["Done"] = "Mission Removed From Favourite";
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
                // TempData["Done"] = "Mission Added to Favourite";
            }
            return Ok("Done");
        }

        [CheckSession]
        [HttpPost]
        public IActionResult ApplyMission(int id)
        {
            var userid = Int64.Parse(HttpContext.Session.GetString("UserId"));
            MissionApplication application = new MissionApplication();
            application = _db.MissionApplications.FirstOrDefault(u => u.MissionId == id && u.UserId == userid && u.DeletedAt == null);
            if (application != null)
            {
                // application in pending
                TempData["Error"] = "Wait some days for Approval !";
                return Ok("You already applied for mission! wait some days to apporove");
            }
            else
            {
                // apply the mission 
                MissionApplication application1 = new MissionApplication();
                application1.MissionId = id;
                application1.UserId = userid;
                application1.CreatedAt = DateTime.Now;
                application1.AppliedAt = DateTime.Now;
                application1.ApprovalStatus = 0;
                _db.MissionApplications.Add(application1);
                _db.SaveChanges();
                TempData["Done"] = "Applied Success fully";
                return Ok("You Application success fully sended to admin........");
            }
        }

        [CheckSession]
        [HttpGet]
        public IActionResult MissionDetail(int id)
        {
            var obj1 = GetMissionById(id);
            var obj2 = _db.MissionDocuments.Where(u => u.MissionId == id && u.DeletedAt == null).ToList();
            var usersid = _db.MissionApplications.Where(u => u.MissionId == id && u.ApprovalStatus == 1 && u.DeletedAt == null).Select(p => p.UserId).ToList();
            var obj3 = _db.Users.Where(u => usersid.Contains(u.UserId)).ToList();
            var obj4 = relatedmission(id);
            var imgs = _db.MissionMedia.Where(i => i.MissionId == id && i.DeletedAt == null && i.Status == 1).Select(p => p.MediaPath).ToList();


            MissionDetail mission = new MissionDetail();
            mission.missioncardDetail = obj1;
            mission.SkillName = string.Join(", ", _db.Skills.Where(u => obj1.MissionSkill.Contains(u.SkillId)).Select(p => p.SkillName).ToList());
            mission.totalUserWhoRate = _db.MissionRatings.Where(m => m.MissionId == id).ToList().Count();
            mission.MissionImages = imgs;
            var temp = _db.MissionRatings.FirstOrDefault(p => p.MissionId == id && p.UserId == Int64.Parse(HttpContext.Session.GetString("UserId")));
            if (temp != null)
                mission.MyRating = temp.Rating;
            else
                mission.MyRating = 0;

            switch (obj1.Availability)
            {
                case 0:
                    mission.AvailableTime = "Not Decided";
                    break;
                case 1:
                    mission.AvailableTime = "Daily";
                    break;
                case 2:
                    mission.AvailableTime = "Weekly";
                    break;
                case 3:
                    mission.AvailableTime = "Weekend";
                    break;                
            }

            if (obj2 != null)
                mission.missionDocuments = obj2;
            if (obj3 != null)
                mission.volunteer = obj3;
            mission.missionCards = obj4;

            return View(mission);
        }


        [HttpPost]
        public string RateMission(long Mid,int Rating)
        {
            var temp = _db.MissionRatings.FirstOrDefault(u => u.MissionId == Mid && u.UserId == Int64.Parse(HttpContext.Session.GetString("UserId")) );
            if(temp != null)
            {
                temp.Rating = Rating;
                temp.UpdatedAt = DateTime.Now;
                _db.MissionRatings.Update(temp);
                _db.SaveChanges();
            }
            else
            {
                MissionRating missionRating = new MissionRating();
                missionRating.MissionId = Mid;
                missionRating.Rating = Rating;
                missionRating.UserId = Int64.Parse(HttpContext.Session.GetString("UserId"));
                missionRating.CreatedAt = DateTime.Now;
                _db.MissionRatings.Add(missionRating);
                _db.SaveChanges();
            }
            return ("Done");
        }

        #region Getting Related Missions for Mission details Page
        public List<MissionCard> relatedmission(int id)
        {
            var obj = GetMissionById(id);
            
            // FOR SAME CITY
            var list = _db.Missions.Where(u => u.CityId == obj.CityId && u.MissionId != id).Select(p => p.MissionId).ToList();
            if (list.Count > 0)
            {
                return Threecards(list);
            }
            list.Clear();

            // FOR SAME COUNTRY
            list = _db.Missions.Where(u => u.CountryId == obj.CountryId && u.MissionId != id).Select(p => p.MissionId).ToList();
            if (list.Count > 0)
            {
                return Threecards(list);
            }
            list.Clear();

            // FOR SAME THEME
            list = _db.Missions.Where(u => u.ThemeId == obj.ThemeId && u.MissionId != id).Select(p => p.MissionId).ToList();
            if (list.Count > 0)
            {
                return Threecards(list);
            }
            list.Clear();

            // FIRST THREE IF NOT ANY FOUND
            list = _db.Missions.Select(p => p.MissionId).ToList();
            return Threecards(list);
        }

        public List<MissionCard> Threecards(List<long> list)
        {
            List<MissionCard> temp = new List<MissionCard>();
            for (int i = 0; i < (list.Count > 3 ? 3 : list.Count); i++)
            {
                var tempmission = GetMissionById(list[i]);
                temp.Add(tempmission);
            }
            return temp;
        }
        #endregion



        #region Suggest CoWorker
        [HttpGet]
        public IActionResult SuggestCoWorker(long id)
        {
            var mission = new Mission();
            mission.MissionId = id;
            return PartialView("_CoWorkerPartial", mission);
        }

        [HttpPost]
        public IActionResult SuggestCoWorker(string WorkerEmail, Mission model)
        {
            if (WorkerEmail != null)
            {
                var mission = _db.Missions.FirstOrDefault(x => x.MissionId == model.MissionId);

                var mailBody = "<h1>" + HttpContext.Session.GetString("UserName") + " Suggest Mission : " + mission.Title + " to You</h1><br><h2><a href='https://localhost:44307/Home/MissionDetail?id= " + model.MissionId + "'>Go Website</a></h2><br><p>Ignore if Not Interested.</p>";

                // Create Email
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("pdparmar91@gmail.com"));
                email.To.Add(MailboxAddress.Parse(WorkerEmail));
                email.Subject = "Co-Worker Suggestion";
                email.Body = new TextPart(TextFormat.Html) { Text = mailBody };

                //  Send Email  
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("pdparmar91@gmail.com", "unmrlfscedyndpvl");
                smtp.Send(email);
                smtp.Disconnect(true);                
            }

            return RedirectToAction("MissionListing","Home");
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}