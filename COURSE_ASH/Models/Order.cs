namespace COURSE_ASH.Models;

public class Order : IAccountBased
{
    public string CurrentLogin { get; set; }

    public int ID { get; set; }
    public List<CartProduct> Products { get; set;}
    public DateTime OrderTime { get; set; }
    public string Address { get; set; }
    public string BuyerName { get; set; }
    public double TotalPrice { get; set;  }
    public string Status { get; set; }

    public static bool CanBeCancelled (string status)
    {
        return status == OrderStatus.AWAITING_CONFIRMATION || status == OrderStatus.ACCEPTED;
    }
}
