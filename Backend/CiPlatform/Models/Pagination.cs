namespace CiPlatform.Models
{
    public class Pagination
    {
        public Pagination()
        {
            Pageindex = 1;
            Pagesize = 10;
            Keyword = "";
        }
        public long Pageindex { get; set; }
        public long Pagesize { get; set; }
        public string Keyword { get; set; }
    }
}
