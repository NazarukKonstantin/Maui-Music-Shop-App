namespace COURSE_ASH.Models
{
    public class AccountData : IAccountBased, IImageDisposable
    {
        public int ID { get; set; }
        public string CurrentLogin { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string ImageLink { get; set; } = string.Empty;

        public double ImageRotation { get; set; } = 0;

        public double ImageScale { get; set; } = 1;

        public bool IsAdmin { get; set; }
        public bool IsBoss { get; set; }
    }
}
