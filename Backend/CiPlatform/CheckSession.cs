using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CiPlatform
{
    public class CheckSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (string.IsNullOrEmpty(filterContext.HttpContext.Session.GetString("UserId")))
            {
                filterContext.Result = new RedirectResult(string.Format("/User/Login"));
            }
        }
    }
}