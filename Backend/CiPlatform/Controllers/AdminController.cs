using CiPlatform.DataModels;
using CiPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using System.IO;

namespace CiPlatform.Controllers
{
    public class AdminController : Controller
    {
        readonly CiPlatformContext _db = new CiPlatformContext();
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdminController(IWebHostEnvironment webHostEnvironment)
        {
            _hostEnvironment = webHostEnvironment;
        }

        #region UserList
       
        [HttpGet]
        public IActionResult Userlist(AdminPageViewModel<User> obj)
        {
            ListOfObject<User> listofuser = new ListOfObject<User>();
            var query = _db.Users.Where(c => (obj.pagination.Keyword == null
            || c.FirstName.ToLower().Contains(obj.pagination.Keyword)
            || c.LastName.ToLower().Contains(obj.pagination.Keyword)
            || c.Email.ToLower().Contains(obj.pagination.Keyword)
            || c.EmployeeId.Contains(obj.pagination.Keyword)
            || c.Department.ToLower().Contains(obj.pagination.Keyword))
            && c.Status == 1).AsQueryable();
            long total = query.Count();

            List<User> list = new List<User>();
            list = query.Skip((int)((obj.pagination.Pageindex - 1) * obj.pagination.Pagesize)).Take((int)obj.pagination.Pagesize).ToList();

            listofuser.Records = list;
            listofuser.total_Records = (int)total;
            obj.listOfObject = listofuser;

            return View(obj);
        }
        #endregion

        #region UpsertUser
        [HttpGet]
        public IActionResult UpsertUser(int id = 0)
        {
            UserVM userVM = new UserVM()
            {
                User = new User(),
                CityList = _db.Cities.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.CityId.ToString()
                }),
                CountryList = _db.Countries.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.CountryId.ToString()
                })
            };
            if (id == 0)
            {
                userVM.User.UserId = 0;
                return View(userVM);
            }            
            userVM.User = _db.Users.FirstOrDefault(u => u.UserId.Equals(id));
            if(userVM.User == null)
            {
                return NotFound();
            }
            return View(userVM);
        }

        [HttpPost]
        public IActionResult UpsertUser(UserVM userVM)
        {
            if (userVM != null)
            {
                if (userVM.User.UserId == 0)
                {
                    userVM.User.CreatedAt = DateTime.Now;
                    userVM.User.Status = 1;
                    _db.Users.Add(userVM.User);
                    _db.SaveChanges();
                }
                else
                {
                    User obj = new User();
                    obj = _db.Users.FirstOrDefault(u => u.UserId.Equals(userVM.User.UserId));
                    if (obj != null)
                    {
                        obj.FirstName = userVM.User.FirstName;
                        obj.LastName = userVM.User.LastName;
                        obj.Email = userVM.User.Email;
                        if (userVM.User.Password != null)
                            obj.Password = userVM.User.Password;
                        obj.EmployeeId = userVM.User.EmployeeId;
                        obj.Department = userVM.User.Department;
                        obj.ProfileText = userVM.User.ProfileText;
                        obj.WhyIVolunteer = userVM.User.WhyIVolunteer;
                        obj.PhoneNumber = userVM.User.PhoneNumber;
                        obj.CityId = userVM.User.CityId;
                        obj.CountryId = userVM.User.CountryId;
                        obj.UpdatedAt = DateTime.Now;
                        _db.Users.Update(obj);
                        _db.SaveChanges();
                    }
                }
                return RedirectToAction("Userlist");
            }
            else
                return NotFound();
        }
        #endregion

        #region DeleteUser
        [HttpPost]
        public IActionResult DeleteUser(int? id)
        {
            if (id == null)
            {
                return BadRequest("");
            }
            var user = _db.Users.FirstOrDefault(g => g.UserId == id);
            if (user == null)
                return BadRequest("User not find");
            user.DeletedAt = DateTime.Now;
            user.Status = 0;
            _db.Users.Update(user);
            _db.SaveChanges();
            return RedirectToAction("Userlist");
        }
        #endregion



        #region Banner List

        [HttpGet]
        public IActionResult Banner(AdminPageViewModel<Banner> obj)
        {
            ListOfObject<Banner> listofbanner = new ListOfObject<Banner>();
            var query = _db.Banners.Where(c => (obj.pagination.Keyword == null
            || c.Title.ToLower().Contains(obj.pagination.Keyword))
            && c.DeletedAt == null).AsQueryable();
            long total = query.Count();

            List<Banner> list = new List<Banner>();
            list = query.Skip((int)((obj.pagination.Pageindex - 1) * obj.pagination.Pagesize)).Take((int)obj.pagination.Pagesize).ToList();

            listofbanner.Records = list;
            listofbanner.total_Records = (int)total;
            obj.listOfObject = listofbanner;

            return View(obj);
        }

        #endregion

        #region ADD/Edit Banner
        [HttpGet]
        public IActionResult UpsertBanner(int id = 0)
        {
            if (id == 0)
            {     
                Banner obj = new Banner();
                obj.BannerId = 0;
                return View(obj);
            }
            var banner = _db.Banners.FirstOrDefault(u => u.BannerId.Equals(id));
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }

        [HttpPost]
        public IActionResult UpsertBanner(Banner banner , IFormFile? file )
        {
            if (banner != null)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"Images\PlatformLanding");
                    var extenstion = Path.GetExtension(file.FileName);
                    
                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                    {
                        file.CopyTo(filesStreams);
                    }
                    banner.Image = @"\Images\PlatformLanding\" + fileName + extenstion;
                }

                if (banner.BannerId == 0)
                {
                    banner.CreatedAt = DateTime.Now;
                    _db.Banners.Add(banner);
                    _db.SaveChanges();
                }
                else
                {
                    var obj = _db.Banners.FirstOrDefault(u => u.BannerId.Equals(banner.BannerId));
                    if (obj != null)
                    {
                        if (obj.Image != null && banner.Image != null)
                        {
                            //this is an edit and we need to remove old image
                            var imagePath = Path.Combine(webRootPath, obj.Image.TrimStart('\\'));
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                            obj.Image = banner.Image;
                        }
                        obj.Title = banner.Title;
                        obj.Text = banner.Text;
                        obj.SortOrder = banner.SortOrder;
                        obj.UpdatedAt = DateTime.Now;
                        _db.Banners.Update(obj);
                        _db.SaveChanges();
                    }
                }
                return RedirectToAction("Banner");
            }
            else
                return NotFound();
        }

        #endregion

        #region Delete Banner

        [HttpPost]
        public IActionResult DeleteBanner(int? id)
        {
            if (id == null)
            {
                return BadRequest("");
            }
            var banner = _db.Banners.FirstOrDefault(g => g.BannerId == id);
            if (banner == null)
                return BadRequest("Banner not found");
            banner.DeletedAt = DateTime.Now;
            _db.Banners.Update(banner);
            _db.SaveChanges();
            return RedirectToAction("Banner");
        }

        #endregion



        #region Mission Skill
        [HttpGet]
        public IActionResult Missionskill(AdminPageViewModel<Skill> obj)
        {
            ListOfObject<Skill> listofskill = new ListOfObject<Skill>();
            var query = _db.Skills.Where(c => (obj.pagination.Keyword == null
            || c.SkillName.ToLower().Contains(obj.pagination.Keyword))
            && c.DeletedAt == null).AsQueryable();
            long total = query.Count();

            List<Skill> list = new List<Skill>();
            list = query.Skip((int)((obj.pagination.Pageindex - 1) * obj.pagination.Pagesize)).Take((int)obj.pagination.Pagesize).ToList();

            listofskill.Records = list;
            listofskill.total_Records = (int)total;
            obj.listOfObject = listofskill;

            return View(obj);
        }
        #endregion

        #region Upsert MissionSkill
        [HttpGet]
        public IActionResult UpsertMissionskill(int id = 0)
        {
            Skill skill = new Skill();
            if (id == 0)
            {
                skill.SkillId = 0;
                return View(skill);
            }
            skill = _db.Skills.FirstOrDefault(u => u.SkillId.Equals(id));
            if (skill == null)
            {
                return NotFound();
            }
            return View(skill);
        }

        [HttpPost]
        public IActionResult UpsertMissionskill(Skill skill)
        {
            if (skill != null)
            {
                if (skill.SkillId == 0)
                {
                    skill.CreatedAt = DateTime.Now;
                    _db.Skills.Add(skill);
                    _db.SaveChanges();
                }
                else
                {
                    Skill obj = new Skill();
                    obj = _db.Skills.FirstOrDefault(u => u.SkillId.Equals(skill.SkillId));
                    if (obj != null)
                    {
                        obj.SkillName = skill.SkillName;
                        obj.Status = skill.Status;
                        obj.UpdatedAt = DateTime.Now;
                        _db.Skills.Update(obj);
                        _db.SaveChanges();
                    }
                }
                return RedirectToAction("Missionskill");
            }
            else
                return NotFound();
        }

        #endregion

        #region Delete Mission skill
        [HttpPost]
        public IActionResult DeleteMissionskill(int? id)
        {
            if (id == null)
            {
                return BadRequest("");
            }
            var skill = _db.Skills.FirstOrDefault(g => g.SkillId == id);
            if (skill == null)
                return BadRequest("Skill not find");
            skill.DeletedAt = DateTime.Now;
            skill.Status = 0;
            _db.Skills.Update(skill);
            _db.SaveChanges();
            return RedirectToAction("Missionskill");
        }
        #endregion



        #region Mission Theme
        [HttpGet]
        public IActionResult Missiontheme(AdminPageViewModel<MissionTheme> obj)
        {
            ListOfObject<MissionTheme> listoftheme = new ListOfObject<MissionTheme>();
            var query = _db.MissionThemes.Where(c => (obj.pagination.Keyword == null
            || c.Title.ToLower().Contains(obj.pagination.Keyword))
            && c.DeletedAt == null).AsQueryable();
            long total = query.Count();

            List<MissionTheme> list = new List<MissionTheme>();
            list = query.Skip((int)((obj.pagination.Pageindex - 1) * obj.pagination.Pagesize)).Take((int)obj.pagination.Pagesize).ToList();

            listoftheme.Records = list;
            listoftheme.total_Records = (int)total;
            obj.listOfObject = listoftheme;

            return View(obj);
        }
        #endregion

        #region Upsert MissionTheme
        [HttpGet]
        public IActionResult UpsertMissiontheme(int id = 0)
        {
            MissionTheme theme = new MissionTheme();
            if (id == 0)
            {
                theme.MissionThemeId = 0;
                return View(theme);
            }
            theme = _db.MissionThemes.FirstOrDefault(u => u.MissionThemeId.Equals(id));
            if (theme == null)
            {
                return NotFound();
            }
            return View(theme);
        }

        [HttpPost]
        public IActionResult UpsertMissiontheme(MissionTheme theme)
        {
            if (theme != null)
            {
                if (theme.MissionThemeId== 0)
                {
                    theme.CreatedAt = DateTime.Now;
                    _db.MissionThemes.Add(theme);
                    _db.SaveChanges();
                }
                else
                {
                    MissionTheme obj = new MissionTheme();
                    obj = _db.MissionThemes.FirstOrDefault(u => u.MissionThemeId.Equals(theme.MissionThemeId));
                    if (obj != null)
                    {
                        obj.Title = theme.Title;
                        obj.Status = theme.Status;
                        obj.UpdatedAt = DateTime.Now;
                        _db.MissionThemes.Update(obj);
                        _db.SaveChanges();
                    }
                }
                return RedirectToAction("Missiontheme");
            }
            else
                return NotFound();
        }

        #endregion

        #region Delete Mission theme
        [HttpPost]
        public IActionResult DeleteMissiontheme(int? id)
        {
            if (id == null)
            {
                return BadRequest("");
            }
            var theme = _db.MissionThemes.FirstOrDefault(g => g.MissionThemeId== id);
            if (theme == null)
                return BadRequest("theme not found");
            theme.DeletedAt = DateTime.Now;
            theme.Status = 0;
            _db.MissionThemes.Update(theme);
            _db.SaveChanges();
            return RedirectToAction("Missiontheme");
        }
        #endregion



        #region CMS Pages

        [HttpGet]
        public IActionResult Cmspage(AdminPageViewModel<CmsPage> obj)
        {
            ListOfObject<CmsPage> listofcms = new ListOfObject<CmsPage>();
            var query = _db.CmsPages.Where(c => (obj.pagination.Keyword == null
            || c.Title.ToLower().Contains(obj.pagination.Keyword))
            && c.DeletedAt == null).AsQueryable();
            long total = query.Count();

            List<CmsPage> list = new List<CmsPage>();
            list = query.Skip((int)((obj.pagination.Pageindex - 1) * obj.pagination.Pagesize)).Take((int)obj.pagination.Pagesize).ToList();

            listofcms.Records = list;
            listofcms.total_Records = (int)total;
            obj.listOfObject = listofcms;

            return View(obj);
        }

        #endregion

        #region Upsert CMS Page

        [HttpGet]
        public IActionResult UpsertCmspage(int id = 0)
        {
            CmsPage cmsPage = new CmsPage();
            if (id == 0)
            {
                cmsPage.CmsPageId = 0;
                return View(cmsPage);
            }
            cmsPage = _db.CmsPages.FirstOrDefault(u => u.CmsPageId.Equals(id));
            if (cmsPage == null)
            {
                return NotFound();
            }
            return View(cmsPage);
        }

        [HttpPost]
        public IActionResult UpsertCmspage(CmsPage cmsPage)
        {
            if (cmsPage != null)
            {
                if (cmsPage.CmsPageId == 0)
                {
                    cmsPage.CreatedAt = DateTime.Now;
                    _db.CmsPages.Add(cmsPage);
                    _db.SaveChanges();
                }
                else
                {
                    CmsPage obj = new CmsPage();
                    obj = _db.CmsPages.FirstOrDefault(u => u.CmsPageId.Equals(cmsPage.CmsPageId));
                    if (obj != null)
                    {
                        obj.Title = cmsPage.Title;
                        obj.Description = cmsPage.Description;
                        obj.Slug = cmsPage.Slug;
                        obj.Status = cmsPage.Status;
                        obj.UpdatedAt = DateTime.Now;
                        _db.CmsPages.Update(obj);
                        _db.SaveChanges();
                    }
                }
                return RedirectToAction("Cmspage");
            }
            else
                return NotFound();
        }

        #endregion

        #region Delete CMS Pages
        [HttpPost]
        public IActionResult DeleteCmspage(int? id)
        {
            if (id == null)
            {
                return BadRequest("");
            }
            var obj = _db.CmsPages.FirstOrDefault(g => g.CmsPageId == id);
            if (obj == null)
                return BadRequest("CMS Page not found");
            obj.DeletedAt = DateTime.Now;
            _db.CmsPages.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("Cmspage");
        }
        #endregion



        #region Mission List 

        [HttpGet]
        public IActionResult Mission(AdminPageViewModel<Mission> obj)
        {
            ListOfObject<Mission> listofmision = new ListOfObject<Mission>();
            var query = _db.Missions.Where(c => (obj.pagination.Keyword == null
            || c.Title.ToLower().Contains(obj.pagination.Keyword)
            || c.StartDate.ToString().Contains(obj.pagination.Keyword)
            || c.EndDate.ToString().Contains(obj.pagination.Keyword))
            && c.DeletedAt == null).AsQueryable();
            long total = query.Count();

            List<Mission> list = new List<Mission>();
            list = query.Skip((int)((obj.pagination.Pageindex - 1) * obj.pagination.Pagesize)).Take((int)obj.pagination.Pagesize).ToList();

            listofmision.Records = list;
            listofmision.total_Records = (int)total;
            obj.listOfObject = listofmision;

            return View(obj);
        }

        #endregion

        #region Upsert Mission Page

        [HttpGet]
        public IActionResult UpsertMission(int id = 0)
        {
            MissionVM mission = new MissionVM()
            {
                Mission = new Mission(),
                CityList = _db.Cities.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.CityId.ToString()
                }),
                CountryList = _db.Countries.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.CountryId.ToString()
                }),
                ThemeList = _db.MissionThemes.Select(i => new SelectListItem
                {
                    Text = i.Title,
                    Value = i.MissionThemeId.ToString()
                }),
                SkillList = _db.Skills.Select(i => new SelectListItem
                {
                    Text = i.SkillName,
                    Value = i.SkillId.ToString(),
                    Selected = false
                }),
            };
            if (id == 0)
            {
                mission.Mission.MissionId = 0;
                return View(mission);
            }
            mission.Mission = _db.Missions.FirstOrDefault(u => u.MissionId.Equals(id));        

            if (mission.Mission == null)
            {
                return NotFound();
            }
            return View(mission);
        }

        [HttpPost]
        public IActionResult UpsertMission(MissionVM missionVM , List<IFormFile>? images , List<IFormFile>? files)
        {
            if (missionVM.Mission != null)
            {
                if (missionVM.Mission.MissionId == 0)
                {
                    missionVM.Mission.CreatedAt = DateTime.Now;
                    _db.Missions.Add(missionVM.Mission);
                    _db.SaveChanges();

                    foreach(var item in missionVM.SkillList)
                    {
                        MissionSkill missionSkill = new MissionSkill();
                        if(item.Selected == true)
                        {
                            missionSkill.MissionId = missionVM.Mission.MissionId;
                            missionSkill.SkillId = Int64.Parse(item.Value);
                            missionSkill.CreatedAt = DateTime.Now;
                            _db.Add(missionSkill);
                            _db.SaveChanges();
                        }
                    }             
                }
                else
                {
                    Mission obj = new Mission();
                    obj = _db.Missions.FirstOrDefault(u => u.MissionId.Equals(missionVM.Mission.MissionId));
                    if (obj != null)
                    {
                        obj.Title = missionVM.Mission.Title;
                        obj.Description = missionVM.Mission.Description;
                        obj.CityId = missionVM.Mission.CityId;
                        obj.CountryId = missionVM.Mission.CountryId;
                        obj.ThemeId = missionVM.Mission.ThemeId;
                        obj.Status = missionVM.Mission.Status;
                        obj.UpdatedAt = DateTime.Now;
                        _db.Missions.Update(obj);
                        _db.SaveChanges();
                    }
                    foreach (var item in missionVM.SkillList)
                    {
                        MissionSkill missionSkill = new MissionSkill();
                        if (item.Selected == true)
                        {
                            missionSkill.MissionId = missionVM.Mission.MissionId;
                            missionSkill.SkillId = Int64.Parse(item.Value);
                            missionSkill.CreatedAt = DateTime.Now;
                            _db.Add(missionSkill);
                            _db.SaveChanges();
                        }
                        else
                        {
                            missionSkill = _db.MissionSkills.FirstOrDefault(u => u.MissionId == missionVM.Mission.MissionId && u.SkillId == Int64.Parse(item.Value));
                        }
                    }

                }


                string webRootPath = _hostEnvironment.WebRootPath;
                                
                if (images != null)
                {
                    foreach (var image in images)
                    {
                        MissionMedium missionMedium = new MissionMedium();
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"Images\MissionPhotos");
                        var extenstion = Path.GetExtension(image.FileName);

                        using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                        {
                            image.CopyTo(filesStreams);
                        }
                        missionMedium.MissionId = missionVM.Mission.MissionId;
                        missionMedium.MediaPath = @"\Images\MissionPhotos\" + fileName + extenstion;
                        missionMedium.MediaName = fileName;
                        missionMedium.MediaType = extenstion;
                        missionMedium.CreatedAt = DateTime.Now;
                        _db.MissionMedia.Add(missionMedium);
                        _db.SaveChanges();
                    }
                }

                if (files != null)
                {
                    foreach (var doc in files)
                    {
                        MissionDocument missiondoc = new MissionDocument();
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"Documents\Mission");
                        var extenstion = Path.GetExtension(doc.FileName);

                        using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                        {
                            doc.CopyTo(filesStreams);
                        }
                        missiondoc.MissionId = missionVM.Mission.MissionId;
                        missiondoc.DocumentPath = @"\Documents\Mission\" + fileName + extenstion;
                        missiondoc.DocumentName = fileName;
                        missiondoc.DocumentType = extenstion;
                        missiondoc.CreatedAt = DateTime.Now;
                        _db.MissionDocuments.Add(missiondoc);
                        _db.SaveChanges();
                    }
                }
                return RedirectToAction("Mission");
            }
            else
                return NotFound();
        }

        #endregion

        #region Delete Mission
        [HttpPost]
        public IActionResult DeleteMission(int? id)
        {
            if (id == null)
            {
                return BadRequest("");
            }
            var obj = _db.Missions.FirstOrDefault(g => g.MissionId == id);
            if (obj == null)
                return BadRequest("Mission not found");
            obj.DeletedAt = DateTime.Now;
            obj.Status = 0;
            _db.Missions.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("Mission");
        }
        #endregion


        
        #region Mission app
        [HttpGet]
        public IActionResult Missionapp(MissionApplicationVM obj)
        {
            ListOfObject<MissionApplication> listofapp = new ListOfObject<MissionApplication>();
            var query = _db.MissionApplications.Where(c => (obj.pagination.Keyword == null
            || c.MissionId.ToString().Contains(obj.pagination.Keyword)
            || c.UserId.ToString().Contains(obj.pagination.Keyword)
            || c.User.FirstName.ToLower().Contains(obj.pagination.Keyword)
            || c.Mission.Title.ToLower().Contains(obj.pagination.Keyword))
            && c.ApprovalStatus == 0).AsQueryable();
            long total = query.Count();

            var list = query.Skip((int)((obj.pagination.Pageindex - 1) * obj.pagination.Pagesize)).Take((int)obj.pagination.Pagesize).ToList();
            obj.user = _db.Users.ToList();
            obj.mission = _db.Missions.ToList();

            listofapp.Records = list;
            listofapp.total_Records = (int)total;
            obj.application = listofapp;
            return View(obj);
        }
        #endregion

        #region Mission application status
        [HttpGet]
        public IActionResult UpsertMissionapp(int id ,bool isapproved)
        {
            MissionApplication application = new MissionApplication();
            if (id == 0)
            {
                application.MissionApplicationId = 0;
                return View(application);
            }
            application = _db.MissionApplications.FirstOrDefault(u => u.MissionApplicationId.Equals(id));
            if (application == null)
            {
                return NotFound();
            }
            if(isapproved)
                application.ApprovalStatus = 1;
            else
                application.ApprovalStatus = 0;
            _db.Update(application);
            _db.SaveChanges();
            return RedirectToAction("Missionapp");
        }
        #endregion

    }
}
