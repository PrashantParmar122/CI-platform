using CiPlatform.DataModels;

namespace CiPlatform.Models
{
    public class StoryDetailVM
    {
        public Story story { get; set; }
        public User user { get; set; }
        public Mission mission { get; set; }
        public List<StoryMedium> storyMedia { get; set; }
    }
}
