namespace API.Models
{
    public class UserAgePlusTwenty
    {
        public Guid? UserId { get; set; }
        public int OriginalAge { get; set; }
        public int AgePlusTwenty { get; set; }
    }
}
