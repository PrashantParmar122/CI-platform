using CiPlatform.DataModels;

namespace CiPlatform.Models
{
    public class MissionDetail
    {
        public MissionCard missioncardDetail { get; set; }
        public List<MissionDocument> missionDocuments { get; set; }
        public List<User> volunteer { get; set; }
        public List<MissionCard> missionCards { get; set; }
        public string AvailableTime { get; set; }
        public string SkillName { get; set; }
        public long totalUserWhoRate { get; set; }
        public int MyRating { get; set; }
        public List<string> MissionImages { get; set; }
    }
}
