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
    public string PhoneNumber { get; set; }
    public string Status { get; set; }

    private static readonly string _phoneNumberPattern = @"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$";
    public static bool IsPhoneNumber(string str)
    {
        return Regex.IsMatch(str, _phoneNumberPattern);
    }
    public static bool CanBeCancelled(string status)
    {
        return status == OrderStatus.AWAITING_CONFIRMATION || status == OrderStatus.ACCEPTED;
    }
}
