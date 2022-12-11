namespace COURSE_ASH.Models;

public class Order : IDBItem, IAccountBased
{
    public int ID { get; set; }
    public string UserName { get; set; }

    public List<Product> Products { get; }
    public DateTime OrderTime { get; }
    public string BuyerName { get; }
    public double TotalPrice { get; }

    public Order(List<Product> products, DateTime orderTime, string buyerName, double totalPrice, int orderID)
    {
        Products = products;
        OrderTime = orderTime;
        BuyerName = buyerName;
        TotalPrice = totalPrice;
        ID = orderID;
    }
}
