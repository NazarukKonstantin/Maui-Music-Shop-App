namespace COURSE_ASH.Models
{
    public class AccountData : IDBItem, IAccountBased
    {
        public int ID { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
