namespace COURSE_ASH.Services;

public class OrderService
{
    public async Task CheckoutAsync(string currentLogin, List<CartProduct> products, string address, double totalPrice)
    {
        var orders = await DataStorageService<Order>.GetItemListAsync();
        int newID = (from order in orders select order.ID)?.Max() ?? 1;
        Order newOrder = new Order
        {
            ID = newID,
            CurrentLogin = currentLogin,
            Products = products,
            OrderTime = DateTime.Now,
            TotalPrice = totalPrice,
            Address = address,
            Status = OrderStatus.AWAITING_CONFIRMATION,
        };

        await DataStorageService<Order>.AddItemAsync(newOrder);
    }
    public async Task<List<Order>> GetOrdersAsync(string currentLogin)
    {
        var orders = await DataStorageService<Order>.GetItemsByAsync(nameof(Order.CurrentLogin), currentLogin);

        return orders.Any() ? orders.ToList() : new List<Order>();
    }
    public async Task<Order> ChangeStatusAsync(int ID, string status)
    {
        var order = await DataStorageService<Order>.GetItemByAsync(nameof(Order.ID), ID);
        if (order is null) return null;
        order.Status = status;
        await DataStorageService<Order>.UpdateItemAsync(order, nameof(Order.ID), ID);
        return order;
    }
    public async Task<List<Order>> GetAllOrdersAsync()
    {
        var orders = await DataStorageService<Order>.GetItemListAsync();

        return orders.Any() ? orders.ToList<Order>() : new List<Order>();
    }
}
