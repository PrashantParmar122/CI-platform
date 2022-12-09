using CiPlatform.DataModels;

namespace CiPlatform.Models
{
    public class MissionCard 
    {
        public MissionCard()
        {

        }

        public MissionCard(Mission mission , MissionTheme missionTheme , Country country , City city , MissionMedium missionMedium ,List<long> obj)
        {
            MissionId = mission.MissionId;
            ThemeId = missionTheme.MissionThemeId;
            ThemeName = missionTheme.Title;
            CountryId = country.CountryId;
            CountryName = country.Name;
            CityId = city.CityId;
            CityName = city.Name;
            MediaName = missionMedium.MediaName;
            MediaType = missionMedium.MediaType;
            MediaPath = missionMedium.MediaPath;
            MissionTitle = mission.Title;
            Availability = mission.Availability;
            OrganizationName = mission.OrganizationName;
            OrganizationDetail = mission.OrganizationDetail;
            StartDate = mission.StartDate;
            EndDate = mission.EndDate;
            Deadline = mission.Deadline;
            ShortDescription = mission.ShortDescription;
            Description = mission.Description;
            TotalSeat = mission.TotalSeat;
            MissionType = mission.MissionType;
            MissionSkill = obj;
            CreatedAt = mission.CreatedAt;
        }

        public long MissionId { get; set; }
        public long ThemeId { get; set; }
        public string? ThemeName { get; set; }
        public long CityId { get; set; }
        public string CityName { get; set; } = null!;
        public long CountryId { get; set; }
        public string CountryName { get; set; } = null!;
        public List<long> MissionSkill { get; set; }
        public string MissionTitle { get; set; } = null!;
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? Deadline { get; set; }
        public long? TotalSeat { get; set; }
        public int MissionType { get; set; }
        public string? OrganizationName { get; set; }
        public string? OrganizationDetail { get; set; }
        public int? Availability { get; set; }
        public string? GoalObjectiveText { get; set; }
        public int GoalValue { get; set; }
        public string? MediaName { get; set; }
        public string? MediaType { get; set; }
        public string? MediaPath { get; set; }
        public bool ismissionfav { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
