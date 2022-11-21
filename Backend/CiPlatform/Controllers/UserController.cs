using CiPlatform.DataModels;
using CiPlatform.Models;
using Microsoft.AspNetCore.Mvc;

namespace CiPlatform.Controllers
{
    public class UserController : Controller
    {
        readonly CiPlatformContext _db = new CiPlatformContext();
        [HttpGet]
        public IActionResult Login()
        {
            LoginPageViewModel loginPageViewModel = new LoginPageViewModel();
            loginPageViewModel.banner = _db.Banners.ToList();
            return View(loginPageViewModel);
        }

        [HttpPost]
        public IActionResult Login(LoginPageViewModel obj)
        {
            var user = _db.Admins.FirstOrDefault(u =>u.Email.Equals(obj.Email.ToLower()) && u.Password.Equals(obj.Password));
            if(user != null)
            {
                return RedirectToAction("Userlist" , "Admin");
            }
            var user1 = _db.Users.FirstOrDefault(u => u.Email.Equals(obj.Email.ToLower()) && u.Password.Equals(obj.Password));
            if (user1 != null)
                return RedirectToAction("Index", "Home");            
            else
                return RedirectToAction("Login", "User");
        }

        // Register Controllers

        public IActionResult Register()
        {
            RegisterPageViewModel registerPageViewModel = new RegisterPageViewModel();
            registerPageViewModel.banner = _db.Banners.ToList();
            return View(registerPageViewModel);
        }

        [HttpPost]
        public IActionResult Register(RegisterPageViewModel obj)
        {
            if (obj != null)
            {
                User user = new User();
                user.Email = obj.Email;
                user.FirstName = obj.FirstName;
                user.LastName = obj.LastName;
                user.PhoneNumber = obj.PhoneNumber;
                user.Password = obj.Password;
                user.CityId = 1;
                user.CountryId = 1;
                user.CreatedAt = DateTime.Now;
                user.Status = 1;
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Login", "User");
            }
            else
            {
                return RedirectToAction("Register", "User");
            }
        }

        // Lost-Password

        public IActionResult Lost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Lost(User obj)
        {
            if (obj.Email != null)
            {
                var objfromdb = _db.Users.FirstOrDefault(u => u.Email.ToLower() == obj.Email.ToLower().Trim());
                if (objfromdb != null)
                {
                    long i = objfromdb.UserId;
                    return RedirectToAction("Reset", "User", new { i });
                }
                return View(obj);
            }
            else
            {
                return View(obj);
            }
        }


        // Reset-Password

        public IActionResult Reset(long i)
        {
            User user = new User();
            user = _db.Users.FirstOrDefault(u => u.UserId == i);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Lost", "User");
        }

        [HttpPost]
        public IActionResult Reset(User obj)
        {
            if (obj != null)
            {
                User user = new User();
                user = _db.Users.FirstOrDefault(u => u.UserId == obj.UserId);
                user.Password = obj.Password;
                _db.Update(user);
                _db.SaveChanges();
                //Change password
                return RedirectToAction("Login", "User");
            }
            else
            {
                return RedirectToAction("Lost", "User");
            }
        }
    }
}
