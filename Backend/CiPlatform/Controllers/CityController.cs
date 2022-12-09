using CiPlatform.DataModels;
using CiPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CiPlatform.Controllers
{
    public class CityController : Controller
    {
        readonly CiPlatformContext _db = new CiPlatformContext();

        [HttpGet]
        public JsonResult GetCity(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var temp = _db.Cities.Where(x => x.DeletedAt == null && x.CountryId == id).AsEnumerable().ToList();
            foreach (var item in temp)
            {
                list.Add(new SelectListItem() { Text = item.Name, Value = item.CityId.ToString() });
            }
            return Json(list);
        }
    }
}
