namespace COURSE_ASH.Models;

public class Review : IAccountBased
{
    public string CurrentLogin { get; set; }
    public string Text { get; set; }
    public int Rating { get; set; }
}
