using CiPlatform.DataModels;
using CiPlatform.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace CiPlatform.Controllers
{
    public class UserController : Controller
    {
        public CiPlatformContext _db = new CiPlatformContext();

        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            LoginPageViewModel loginPageViewModel = new LoginPageViewModel();
            loginPageViewModel.banner = _db.Banners.Where(u => u.DeletedAt == null).AsQueryable().ToList();
            return View(loginPageViewModel);
        }

        [HttpPost]
        public IActionResult Login(LoginPageViewModel obj)
        {
            var user = _db.Admins.FirstOrDefault(u =>u.Email.Equals(obj.Email.ToLower()) && u.Password.Equals(obj.Password));
            if(user != null)
            {
                HttpContext.Session.SetString("UserId", user.AdminId.ToString());
                HttpContext.Session.SetString("UserName", user.FirstName);
                return RedirectToAction("Userlist" , "Admin");
            }
            var user1 = _db.Users.FirstOrDefault(u => u.Email.Equals(obj.Email.ToLower()) && u.Password.Equals(obj.Password));
            
            if (user1.Status == 0)
            {
                TempData["ErrorMes"] = "This user is deactivated";
                return RedirectToAction("Login" ,"User");
            }

            HttpContext.Session.SetString("UserId", user1.UserId.ToString());
            HttpContext.Session.SetString("UserName", user1.FirstName);

            if (user1 != null)
                return RedirectToAction("Index", "Home");            
            else
                return RedirectToAction("Login", "User");
        }
        #endregion

        
        
        #region Register
        public IActionResult Register()
        {
            RegisterPageViewModel registerPageViewModel = new RegisterPageViewModel();
            registerPageViewModel.banner = _db.Banners.Where(u => u.DeletedAt == null).AsQueryable().ToList();
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
        #endregion



        #region Lost 
        public IActionResult Lost()
        {
            LostPasswordVM model = new LostPasswordVM();
            model.banner = _db.Banners.Where(u => u.DeletedAt == null).AsQueryable().ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult Lost(LostPasswordVM obj)
        {

            var user = _db.Users.FirstOrDefault(u => u.Email.Equals(obj.Email.ToLower()) && u.DeletedAt == null);

            if (user == null)
            {
                // Error User not found
                return RedirectToAction("Login" , "User");
            }

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[16];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var tokenString = new String(stringChars);

            PasswordReset entry = new PasswordReset();
            entry.Email = obj.Email;
            entry.Token = tokenString;
            entry.CreatedAt = DateTime.Now;
            _db.PasswordResets.Add(entry);
            _db.SaveChanges();

            var mailBody = "<h1>Click Below link to reset your password</h1><br><h2><a href='" + "https://localhost:44307/User/Reset?Token=" + tokenString + "&Email=" +obj.Email+ "'>Reset Password</a></h2>";

            // Create Email
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("pdparmar91@gmail.com"));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Reset Your Password";
            email.Body = new TextPart(TextFormat.Html) { Text = mailBody };

            //  Send Email  
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("pdparmar91@gmail.com", "unmrlfscedyndpvl");
            smtp.Send(email);
            smtp.Disconnect(true);

            // Check your Email to Reset password.....
            return RedirectToAction("Login", "User");
        }
        #endregion



        #region Reset
        [HttpGet]
        public IActionResult Reset(String Token , String Email )
        {
            ResetPasswordVM model = new ResetPasswordVM();
            model.banner = _db.Banners.Where(u => u.DeletedAt == null).AsQueryable().ToList();
            model.Token = Token;
            model.Email = Email;
            return View(model);
        }

        [HttpPost]
        public IActionResult Reset(ResetPasswordVM model) 
        {
            if(model.Token != null && model.Email != null)
            {
                var resetPassUser = _db.PasswordResets.OrderByDescending(x => x.CreatedAt).FirstOrDefault(x => x.Token == model.Token && x.Email == model.Email);

                DateTime currentTime = DateTime.Now;
                TimeSpan diffrenceTime = (TimeSpan)(currentTime - resetPassUser.CreatedAt);
                if (diffrenceTime.TotalHours <= 4.0)
                {
                    var user = _db.Users.FirstOrDefault(x => x.Email.Equals(resetPassUser.Email) && x.DeletedAt == null);
                    if(user != null)
                    {
                        user.Password = model.Password;
                        _db.Users.Update(user);
                        _db.SaveChanges();
                    }
                    return RedirectToAction("Login", "User");

                }
                else
                {
                    // Token Expire messages
                    return RedirectToAction("Login", "User");
                }
            }
            else
            {
                return View();
            }
            
        }
        #endregion
    
    }
}
