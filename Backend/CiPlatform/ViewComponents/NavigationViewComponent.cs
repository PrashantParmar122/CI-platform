using Microsoft.AspNetCore.Mvc;

namespace CiPlatform.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
