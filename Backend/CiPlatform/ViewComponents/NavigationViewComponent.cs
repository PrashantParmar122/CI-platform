using CiPlatform.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace CiPlatform.ViewComponents
{
    [CheckSession]
    public class NavigationViewComponent : ViewComponent
    {
        CiPlatformContext _db = new CiPlatformContext();
        public IViewComponentResult Invoke()
        {            
            var a = _db.Users.FirstOrDefault(u => u.UserId == Int64.Parse(HttpContext.Session.GetString("UserId")));
            return View(a);
        }
    }
}
