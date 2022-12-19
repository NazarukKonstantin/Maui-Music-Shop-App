namespace COURSE_ASH.Models
{
    public class AccountData : IAccountBased, IImageDisposable
    {
        public int ID { get; set; }
        public string CurrentLogin { get; set; }
        public string PasswordSHA256 { get; set; }
        public Roles Role { get; set; }
        public string ImageLink { get; set; }
    }
}
