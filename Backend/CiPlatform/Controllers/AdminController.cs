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
                return BadRequest("Banner not find");
            banner.DeletedAt = DateTime.Now;
            _db.Banners.Update(banner);
            _db.SaveChanges();
            return RedirectToAction("Banner");
        }

        #endregion
    }
}
