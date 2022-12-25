using System.Text.RegularExpressions;

namespace COURSE_ASH.Models;

public class Order : IAccountBased
{
    public string CurrentLogin { get; set; }

    public int ID { get; set; }
    public List<CartProduct> Products { get; set;}
    public DateTime OrderTime { get; set; }
    public string Address { get; set; }
    public double TotalPrice { get; set;  }
    public string Email { get; set; }
    public string Status { get; set; }

    private static readonly string _emailPattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
    public static bool IsEmail(string str)
    {
        return Regex.IsMatch(str, _emailPattern, RegexOptions.IgnoreCase);
    }
    public static bool CanBeCancelled(string status)
    {
        return status == OrderStatus.AWAITING_CONFIRMATION || status == OrderStatus.ACCEPTED;
    }
}
