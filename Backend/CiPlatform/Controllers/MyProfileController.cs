using CiPlatform.DataModels;
using CiPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace CiPlatform.Controllers
{
    public class MyProfileController : Controller
    {
        readonly CiPlatformContext _db = new CiPlatformContext();


        #region My Profile Page
        [CheckSession]
        [HttpGet]
        public IActionResult MyProfile()
        {
            UserVM userVM = new UserVM();
            userVM.CountryList = _db.Countries.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CountryId.ToString()
            });
            userVM.SkillList = _db.Skills.Select(i => new SelectListItem
            {
                Text = i.SkillName,
                Value = i.SkillId.ToString()
            });
            userVM.User = new User();
            userVM.User = _db.Users.FirstOrDefault(u => u.UserId.Equals(Int64.Parse(HttpContext.Session.GetString("UserId"))));
            if (userVM.User == null)
            {
                return NotFound();
            }
            var Listofcity = _db.Cities.Where(i => i.CountryId == userVM.User.CountryId).AsQueryable().ToList();

            userVM.CityList = Listofcity.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CityId.ToString()
            });
            userVM.Skillname = _db.UserSkills.Where(i => i.UserId == userVM.User.UserId && i.DeletedAt == null).Select(j => j.SkillId.ToString()).ToList();
            return View(userVM);
        }
        #endregion


        #region Edit Profile
        [CheckSession]
        [HttpPost]
        public IActionResult MyProfile(UserVM userVM)
        {
            if(userVM == null)
            {
                return BadRequest();
            }
            User user = new User();
            user = _db.Users.FirstOrDefault(u => u.UserId == userVM.User.UserId);
            if(user != null)
            {
                user.UserId = userVM.User.UserId;
                user.FirstName = userVM.User.FirstName;
                user.LastName = userVM.User.LastName;
                user.EmployeeId = userVM.User.EmployeeId;
                user.PhoneNumber = userVM.User.PhoneNumber;
                user.Title = userVM.User.Title;
                user.Department = userVM.User.Department;
                user.ProfileText = userVM.User.ProfileText;
                user.CountryId = userVM.User.CountryId;
                user.CityId = userVM.User.CityId;
                user.LinkedInUrl = userVM.User.LinkedInUrl;
                user.WhyIVolunteer = userVM.User.WhyIVolunteer;
                user.UpdatedAt = DateTime.Now;
                _db.Update(user);
                _db.SaveChanges();
            }
            
            #region Edit / Add User Skill
            if (userVM.Skillname != null)
            {
                var oldlist = new List<int>();
                oldlist = _db.UserSkills.Where(u => u.UserId == userVM.User.UserId && u.DeletedAt == null).Select(o => (int)o.SkillId).AsQueryable().ToList();

                var temp = userVM.Skillname[0];
                var numbers = temp?.Split(',')?.Select(Int32.Parse)?.ToList();
                if (numbers != null)
                {
                    foreach (var n in numbers)
                    {
                        UserSkill userSkill = new UserSkill();
                        userSkill = _db.UserSkills.FirstOrDefault(i => i.UserId == userVM.User.UserId && i.SkillId == n && i.DeletedAt == null);
                        if (userSkill == null)
                        {
                            UserSkill temp1 = new UserSkill();
                            temp1.UserId = userVM.User.UserId;
                            temp1.SkillId = n;
                            temp1.CreatedAt = DateTime.Now;
                            _db.UserSkills.Add(temp1);
                            _db.SaveChanges();
                        }
                        else
                        {
                            oldlist.Remove(n);
                        }
                    }
                }
                foreach (var item in oldlist)
                {
                    UserSkill olduserSkill = new UserSkill();
                    olduserSkill = _db.UserSkills.FirstOrDefault(u => u.SkillId == item && u.UserId == userVM.User.UserId && u.DeletedAt == null);
                    if (olduserSkill != null)
                    {
                        olduserSkill.DeletedAt = DateTime.Now;
                        _db.UserSkills.Update(olduserSkill);
                        _db.SaveChanges();
                    }
                }
            }
            #endregion

            return RedirectToAction("MyProfile", "MyProfile");
        }
        #endregion


        #region Change Password
        [CheckSession]
        [HttpPost]
        public IActionResult ChangePassword(string oldpass , string newpass)
        {
            User user= new User();
            user = _db.Users.FirstOrDefault(u => u.UserId.Equals(Int64.Parse(HttpContext.Session.GetString("UserId"))));
            if (user.Password != oldpass)
            {
                return NotFound("Old Password not matched!");
            }
            else
            {
                user.Password = newpass;
                _db.Users.Update(user);
                _db.SaveChanges();
                return Ok("Password changed successfully......");
            }
        }
        #endregion



        #region Time Sheet Page View
        [CheckSession]
        [HttpGet]
        public IActionResult TimeSheet()
        {
            var uid = Int64.Parse(HttpContext.Session.GetString("UserId"));
            var timesheet = _db.Timesheets.Where(i => i.DeletedAt == null && i.Status == 1 && i.UserId == uid).ToList();
            List<TimeSheetVM> timeSheetVMs = new List<TimeSheetVM>();

            foreach(var t in timesheet)
            {
                TimeSheetVM temp = new TimeSheetVM();
                temp.Timesheetid = t.TimesheetId;
                temp.Missionid = t.MissionId;
                temp.Notes = t.Notes;
                var mission = _db.Missions.FirstOrDefault(i => i.MissionId == t.MissionId);
                temp.Missiontype = mission.MissionType;
                temp.MissionTitle = mission.Title;
                temp.date = t.DateVolunteered;
                if(temp.Missiontype == 2)
                    temp.task = t.Action;
                else
                {
                    temp.min = t.Minutes;
                    temp.hour = t.Hour;
                }
                timeSheetVMs.Add(temp);
            }
            return View(timeSheetVMs);
        }
        #endregion

        #region Upsert Volunteer Sheet
        [CheckSession]
        [HttpGet]
        public IActionResult UpsertTimeSheet(int mtype , int id = 0)
        {
            var uid = Int64.Parse(HttpContext.Session.GetString("UserId"));
            var missionidlist = _db.MissionApplications.Where(i => i.ApprovalStatus == 1 && i.UserId == uid && i.DeletedAt == null).Select(p => p.MissionId).ToList();
            
            TimeSheetVM timeSheetVM = new TimeSheetVM();
            timeSheetVM.Missiontype = mtype;
            timeSheetVM.Timesheetid = id;
            timeSheetVM.date = DateTime.Now;
            if (mtype == 1)
            {
                var TimeMissionList = _db.Missions.Where(i => missionidlist.Contains(i.MissionId) && i.MissionType == 1).ToList();
                timeSheetVM.MissionList = TimeMissionList.Select(i => new SelectListItem
                {
                    Text = i.Title,
                    Value = i.MissionId.ToString()
                });
            }
            else
            {
                var GoalMissionList = _db.Missions.Where(i => missionidlist.Contains(i.MissionId) && i.MissionType == 2).ToList();
                timeSheetVM.MissionList = GoalMissionList.Select(i => new SelectListItem
                {
                    Text = i.Title,
                    Value = i.MissionId.ToString()
                });
            }
            if(timeSheetVM.MissionList == null)
            {
                return BadRequest("Need to be part of mission.....");
            }

            if(id == 0)
                return View(timeSheetVM);
            var temp = _db.Timesheets.FirstOrDefault(i => i.TimesheetId == id);
            timeSheetVM.Missionid = temp.MissionId;
            timeSheetVM.hour = temp.Hour;
            timeSheetVM.min = temp.Minutes;
            timeSheetVM.task = temp.Action;
            timeSheetVM.Notes = temp.Notes;            
            timeSheetVM.date = temp.DateVolunteered;
            return View(timeSheetVM);
        }

        [HttpPost]
        public IActionResult UpsertTimeSheet(TimeSheetVM timeSheetVM)
        {
            var uid = Int64.Parse(HttpContext.Session.GetString("UserId"));
            
            if(timeSheetVM.Timesheetid == 0)
            {
                Timesheet obj = new Timesheet();
                obj.Action = timeSheetVM.task;
                obj.Hour = timeSheetVM.hour;
                obj.Minutes = timeSheetVM.min;
                obj.MissionId = timeSheetVM.Missionid;
                obj.UserId = uid;
                obj.Notes = timeSheetVM.Notes;
                obj.DateVolunteered = timeSheetVM.date;
                obj.CreatedAt = DateTime.Now;
                obj.Status = 1;
                _db.Timesheets.Add(obj);
                _db.SaveChanges();
            }
            else
            {
                var obj = _db.Timesheets.FirstOrDefault(i => i.TimesheetId == timeSheetVM.Timesheetid);
                obj.Action = timeSheetVM.task;
                obj.Hour = timeSheetVM.hour;
                obj.Minutes = timeSheetVM.min;
                obj.MissionId = timeSheetVM.Missionid;
                obj.UserId = uid;
                obj.Notes = timeSheetVM.Notes;
                obj.DateVolunteered = timeSheetVM.date;
                obj.UpdatedAt = DateTime.Now;
                obj.Status = 1;
                _db.Timesheets.Update(obj);
                _db.SaveChanges();
            }
            return RedirectToAction("TimeSheet", "MyProfile");
        }
        #endregion

        #region Delete Time sheet
        [HttpPost]
        public IActionResult DeleteTimeSheet(int id)
        {
            var timesheet = _db.Timesheets.FirstOrDefault(i => i.TimesheetId == id);
            timesheet.DeletedAt = DateTime.Now;
            timesheet.Status = 0;
            _db.Update(timesheet);
            _db.SaveChanges();
            return RedirectToAction("TimeSheet","MyProfile");
        }
        #endregion
    }
}
