using CiPlatform.DataModels;
using CiPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;

namespace CiPlatform.Controllers
{
    public class StoryController : Controller
    {
        readonly CiPlatformContext _db = new CiPlatformContext();
        private readonly IWebHostEnvironment _hostEnvironment;

        public StoryController(IWebHostEnvironment webHostEnvironment)
        {
            _hostEnvironment = webHostEnvironment;
        }

        [CheckSession]
        public IActionResult StoryList()
        {
            List<StoryListVM> storyListVM = new List<StoryListVM>();
            List<Story> stories = new List<Story>();
            stories = _db.Stories.Where(p => p.DeletedAt == null && p.Status == 1).ToList();
            foreach (var item in stories)
            {
                StoryListVM temp = new StoryListVM();
                if(item.Description?.Count() > 90)
                    item.Description = item.Description.Substring(0, 90) + "...";
                temp.story = item;
                var user = _db.Users.FirstOrDefault(u => u.UserId == item.UserId); 
                temp.avatar = user.Avatar;
                temp.VolunteerName = user.FirstName + " " + user.LastName;
                temp.themeName = _db.MissionThemes.FirstOrDefault(p => p.MissionThemeId == _db.Missions.FirstOrDefault(u => u.MissionId == item.MissionId).ThemeId).Title;
                temp.Storyimg = _db.StoryMedia.FirstOrDefault(p => p.StoryId == item.StoryId).Path;
                storyListVM.Add(temp);
            }
            return View(storyListVM);
        }

        [CheckSession]
        [HttpGet]
        public IActionResult UpsertStory()
        {
            UpsertStoryVM upsertStoryVM = new UpsertStoryVM();

            var uid = Int64.Parse(HttpContext.Session.GetString("UserId"));
            var appliedmission = _db.MissionApplications.Where(u => u.UserId == uid && u.DeletedAt == null && u.ApprovalStatus == 1).Select(p => p.MissionId).ToList();
            upsertStoryVM.MissionList = _db.Missions.Where(p => appliedmission.Contains(p.MissionId)).Select(i => new SelectListItem
            {
                Text = i.Title,
                Value = i.MissionId.ToString()
            });
            return View(upsertStoryVM);
        }

        [CheckSession]
        [HttpPost]
        public IActionResult UpsertStory(UpsertStoryVM upsertStoryVM , List<IFormFile>? images)
        {
            var uid = Int64.Parse(HttpContext.Session.GetString("UserId"));

            Story story = new Story();
            story.UserId = uid;
            story.MissionId = upsertStoryVM.MissionId;
            story.Title = upsertStoryVM.Title;
            story.Description = upsertStoryVM.Description;
            story.CreatedAt = DateTime.Now;
            story.Status = 0;
            story.PublishedAt = upsertStoryVM.date;
            story.ViewCount = 0;
            _db.Stories.Add(story);
            _db.SaveChanges();

            string webRootPath = _hostEnvironment.WebRootPath;

            if (images != null)
            {
                foreach (var image in images)
                {
                    StoryMedium storyMedium = new StoryMedium();
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"Images\Story");
                    var extenstion = Path.GetExtension(image.FileName);

                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                    {
                        image.CopyTo(filesStreams);
                    }
                    storyMedium.StoryId = story.StoryId;
                    storyMedium.Path = @"\Images\Story\" + fileName + extenstion;
                    storyMedium.Type = extenstion;
                    storyMedium.CreatedAt = DateTime.Now;
                    _db.StoryMedia.Add(storyMedium);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("StoryList" , "Story");
        }

        [CheckSession]
        public IActionResult StoryDetail(long id)
        {
            var story = _db.Stories.FirstOrDefault(i => i.StoryId == id);
            if(story == null)
                return NotFound();
            story.ViewCount++;
            _db.Update(story);
            _db.SaveChanges();

            var img = _db.StoryMedia.Where(u => u.StoryId == story.StoryId && u.DeletedAt == null).ToList();

            var user = _db.Users.FirstOrDefault(u => u.UserId == story.UserId);

            var mission = _db.Missions.FirstOrDefault(u => u.MissionId == story.MissionId);

            StoryDetailVM storyDetailVM = new StoryDetailVM();
            storyDetailVM.mission = mission;
            storyDetailVM.story = story;
            storyDetailVM.user = user;
            storyDetailVM.storyMedia = img;

            return View(storyDetailVM);
        }

    }
}
