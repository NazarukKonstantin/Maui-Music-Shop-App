namespace COURSE_ASH.Models.Interfaces;

public interface ICRUDable 
{
    public string Key { get; set; }
    public int ID { get; set; }
    public static int TotalAmount { get; set; } = 0;
}
