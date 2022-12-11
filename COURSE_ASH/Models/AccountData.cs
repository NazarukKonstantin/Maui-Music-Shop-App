namespace COURSE_ASH.Models
{
    public class AccountData : IAccountBased
    {
        public int ID { get; set; }

        public string CurrentLogin { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
