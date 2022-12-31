using CiPlatform.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace CiPlatform.Controllers
{
    public class PolicyController : Controller
    {
        readonly CiPlatformContext _db = new CiPlatformContext();

        [CheckSession]
        [HttpGet]
        public IActionResult Policy()
        {
            var policy = _db.CmsPages.Where(i => i.DeletedAt == null && i.Status == 1).ToList();
            return View(policy);
        }
    }
}
