using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COURSE_ASH.Model
{
    public class Order
    {
        public List<Product> Products { get; private set; }
        public int OrderId { get; private set; }
        public DateTime OrderTime { get; private set; }
        public string BuyerName { get; private set; }
        public double TotalCost { get; private set; }

        public Order(List<Product> products, DateTime orderTime, string userName, double totalCost, int orderId)
        {
            Products = products;
            OrderTime = orderTime;
            BuyerName = userName;
            TotalCost = totalCost;
            OrderId = orderId;
        }
    }
}
