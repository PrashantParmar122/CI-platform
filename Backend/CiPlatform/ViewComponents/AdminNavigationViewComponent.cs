using CiPlatform.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace CiPlatform.ViewComponents
{
    [CheckSession]
    public class AdminNavigationViewComponent : ViewComponent
    {
        CiPlatformContext _db = new CiPlatformContext();
        public IViewComponentResult Invoke()
        {
            var a = _db.Admins.FirstOrDefault(u => u.AdminId == Int64.Parse(HttpContext.Session.GetString("UserId")));
            return View(a);
        }
    }
}
