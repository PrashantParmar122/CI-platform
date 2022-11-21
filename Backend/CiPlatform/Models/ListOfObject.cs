namespace CiPlatform.Models
{
    public class ListOfObject<T> where T : class
    {
        public List<T> Records { get; set; }
        public int total_Records { get; set; }        
    }
}
