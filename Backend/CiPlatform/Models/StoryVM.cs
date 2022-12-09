using CiPlatform.DataModels;

namespace CiPlatform.Models
{
    public class StoryVM
    {
        public StoryVM()
        {
            pagination = new Pagination();
        }
        public ListOfObject<Story> story { get; set; }
        public List<Mission> mission { get; set; }
        public List<User> user { get; set; }
        public Pagination pagination { get; set; }
    }
}
