namespace COURSE_ASH.Services;

public class OrderService
{
    public async Task CheckoutAsync(string currentLogin, List<CartProduct> products, string address,double totalPrice)
    {
        int newID = await DataStorageService<Order>.GetIDAsync();
        Order newOrder = new ()
        {
            Products = products,
            OrderTime = DateTime.Now,
            TotalPrice = totalPrice,
            ID = newID,
            Address=address,
            Status = OrderStatus.AWAITING_CONFIRMATION,
            CurrentLogin = currentLogin,
        };

        await DataStorageService<Order>.AddItemAsync(newOrder);
    }
    public async Task<List<Order>> GetOrdersAsync(string currentLogin)
    {
        var orders = await DataStorageService<Order>.GetItemsAsync(nameof(Order.CurrentLogin), currentLogin);

        return orders.Any() ? orders.ToList<Order>() : new List<Order>();
    }
    public async Task<Order> ChangeStatusAsync(int orderID, string status)
    {

        var order = await DataStorageService<Order>.GetItemAsync(nameof(Order.ID),orderID);
        if (order is null) return null;
        order.Status = status;

        await DataStorageService<Order>.UpdateItemAsync(order, nameof(Order.ID), orderID);

        return order;
    }
}
