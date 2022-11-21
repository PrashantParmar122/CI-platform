using CiPlatform.DataModels;
using CiPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Drawing;
using System.Linq;

namespace CiPlatform.Controllers
{
    public class AdminController : Controller
    {
        readonly CiPlatformContext _db = new CiPlatformContext();
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


        [HttpGet]
        public IActionResult UpsertUser(int id = 0)
        {            
            if (id == 0)
            {
                return View();
            }
            User user = new User();
            user = _db.Users.FirstOrDefault(u => u.UserId.Equals(id));
            return View(user);
        }

        [HttpPost]
        public IActionResult UpsertUser(User user)
        {
            if (user != null)
            {
                if (user.UserId == 0)
                {
                    user.CityId = 1;
                    user.CountryId = 1;
                    user.CreatedAt = DateTime.Now;
                    user.Status = 1;
                    _db.Users.Add(user);                    
                    _db.SaveChanges();
                }
                else
                {
                    User obj = new User();
                    obj = _db.Users.FirstOrDefault(u => u.UserId.Equals(user.UserId));
                    if (obj != null)
                    {
                        obj.FirstName = user.FirstName;
                        obj.LastName = user.LastName;
                        obj.Email = user.Email;
                        if(user.Password != null)
                           obj.Password = user.Password;
                        obj.PhoneNumber = user.PhoneNumber;
                        obj.UpdatedAt = DateTime.Now;
                        _db.Users.Update(obj);
                        _db.SaveChanges();
                    }
                }
                return RedirectToAction("Userlist");
            }
            else
                return BadRequest("User not found");
        }
    }
}
