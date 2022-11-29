using CiPlatform.DataModels;

namespace CiPlatform.Models
{
    public class MissionApplicationVM
    {
        public MissionApplicationVM()
        {
            pagination = new Pagination();
        }
        public ListOfObject<MissionApplication> application { get; set; }
        public List<Mission> mission { get; set; }
        public List<User> user { get; set; }
        public Pagination pagination { get; set; }
    }
}
