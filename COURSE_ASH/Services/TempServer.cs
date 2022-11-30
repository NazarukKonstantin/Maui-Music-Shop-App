using System.Text.Json;

namespace COURSE_ASH.Services;

public static class TempServer
{
    static readonly string path = FileSystem.Current.AppDataDirectory;
    static readonly string fullPath = Path.Combine(path, "UserData.json");
    static readonly string ordePath = Path.Combine(path, "OrderData.json");

    static readonly List<Order> _orders;
    static readonly Dictionary<string, string> _accounts;

    static Dictionary<string, string> ReadAccounts()
    {
        Dictionary<string, string> accounts;
        using FileStream fStream = new(fullPath, FileMode.OpenOrCreate);
        try
        {
            accounts = JsonSerializer.Deserialize<Dictionary<string, string>>(fStream);
        }
        catch (Exception)
        {
            accounts = new Dictionary<string, string>();
        }
        return accounts;
    }
    static void WriteAccounts(Dictionary<string, string> accounts)
    {
        using FileStream fs = new(fullPath, FileMode.Create);
        JsonSerializer.Serialize(fs, accounts);
    }
    static TempServer()
    {
        _accounts = ReadAccounts();

        _orders = ReadOrders();
    }
    public async static Task<AccountConfirmator> LogInAsync(string login, string password)
    {
        await Task.Delay(560);
        if (string.IsNullOrEmpty(login)||string.IsNullOrEmpty(password)) return new AccountConfirmator(login,
            AccountAlerts.FIELDS_EMPTY);
        if (!_accounts.ContainsKey(login) || _accounts[login] != password) return new AccountConfirmator(login,
            AccountAlerts.INCORRECT_LOGIN_OR_PASSWORD);
        return new AccountConfirmator(login, "Success");
    }
    public async static Task<AccountConfirmator> CreateAccountAsync(string login, string password, string confirmedPassword)
    {
        await Task.Delay(4000);

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmedPassword))
            return new AccountConfirmator(login, AccountAlerts.FIELDS_EMPTY);
        if (_accounts.ContainsKey(login)) return new AccountConfirmator(login, AccountAlerts.SAME_LOGIN_EXIST);
        if (password != confirmedPassword) return new AccountConfirmator(login, AccountAlerts.PASSWORDS_DONT_MATCH);
        _accounts.Add(login, password);
        WriteAccounts(_accounts);
        return new AccountConfirmator(login, AccountAlerts.SUCCESS);
    }

    public static List<Product> FilterProducts(Product filterProduct, FilterField filterField)
    {
        List<Product> products = new ProductsService().GetProducts();
        return (from p in products where p.Equals(filterProduct,filterField) select p).ToList();
    }

    public static List<Product> FilterProducts<T>(FilterField filterField, T filter)
    {
        List<Product> products = new ProductsService().GetProducts();
        decimal temp = 0;
        if (filter is decimal) 
            if(!decimal.TryParse(filter as string, out temp)) return products;
        return filterField switch
        {
            FilterField.P_TYPE => (from p in products where p.ProductType == filter as string select p).ToList(),
            FilterField.MODEL => (from p in products where p.Model == filter as string select p).ToList(),
            FilterField.INFO => (from p in products where p.Info == filter as string select p).ToList(),
            FilterField.PRICE => (from p in products where p.Price == temp select p).ToList(),
            FilterField.P_IMAGE => (from p in products where p.ProductImage == filter as string select p).ToList(),
            FilterField.RATING => (from p in products where p.Rating == Convert.ToInt32(filter) select p).ToList(),
            _ => products
        };
    }

    public async static Task<bool> Checkout(List<Product> products, string userName,
        DateTime orderTime, double totalPrice)
    {
        int newId = 1;
        foreach (Order order in _orders) newId++;
        var newOrder = new Order(products, orderTime, userName, totalPrice, newId);
        _orders.Add(newOrder);
        await Task.Delay(100);
        WriteOrders(_orders);
        return true;
    }

    public async static Task<List<Order>> GetOrders(string buyerName)
    {
        await Task.Delay(100);
        return (from order in _orders where order.BuyerName == buyerName select order).ToList<Order>();
    }

    static List<Order> ReadOrders()
    {
        List<Order> orders;
        using FileStream fs = new(fullPath, FileMode.OpenOrCreate);
        try
        {
            orders = JsonSerializer.Deserialize<List<Order>>(fs);
        }
        catch (Exception)
        {
            orders = new List<Order>();
        }
        return orders;
    }
    static void WriteOrders(List<Order> orders)
    {
        using FileStream fs = new(fullPath, FileMode.Create);
        JsonSerializer.Serialize(fs, orders);
    }
}
