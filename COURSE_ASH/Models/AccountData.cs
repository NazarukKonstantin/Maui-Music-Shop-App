namespace COURSE_ASH.Models
{
    public class AccountData : IAccountBased, IImageDisposable
    {
        public int ID { get; set; }

        public string CurrentLogin { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public string ImageLink { get; set; }
        public int ImageLinkCounter { get; set; }
    }
}
