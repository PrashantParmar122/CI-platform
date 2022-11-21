namespace CiPlatform.Models
{
    public class AdminPageViewModel<t> where t : class
    {
        public AdminPageViewModel()
        {
            pagination = new Pagination();
        }
        public ListOfObject<t> listOfObject { get; set; }
        public Pagination pagination { get; set; }
    }
}
