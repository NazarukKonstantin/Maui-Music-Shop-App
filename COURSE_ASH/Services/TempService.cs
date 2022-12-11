using System.Text.Json;

namespace COURSE_ASH.Services;

public class TempService
{
    static readonly string orderPath = FileSystem.Current.AppDataDirectory;
    static readonly string fullOrderPath = Path.Combine(orderPath, "OrderData.json");

    static readonly Dictionary<string, List<Product>> _favourites;
    static readonly List<Product> _products;

    static Dictionary<string, List<Product>> ReadFavourites()
    {
        Dictionary<string, List<Product>> products;
        using (FileStream fs = new(fullOrderPath, FileMode.OpenOrCreate))
        {
            try
            {
                products = JsonSerializer.Deserialize<Dictionary<string, List<Product>>>(fs);
            }
            catch (Exception)
            {
                products = new Dictionary<string, List<Product>>();
            }
        }
        return products;
    }
    static void WriteFavourites(Dictionary<string, List<Product>> favourites)
    {
        using (FileStream fs = new(fullOrderPath, FileMode.Create))
        {
            JsonSerializer.Serialize(fs, favourites);
        }
    }

    static TempService()
    {
        _favourites = ReadFavourites();
    }

    public async static Task<List<Product>> GetFavouritesForUserAsync(string currentLogin)
    {
        await Task.Delay(10);
        if (_favourites.ContainsKey(currentLogin))
            return _favourites[currentLogin];
        else return new List<Product>();
    }

    public async static Task SetFavouriteAsync(string userName, int productID)
    {
        await Task.Delay(10);
        if (_favourites.ContainsKey(userName))
            _favourites[userName].AddRange(from product in _products where product.ID == productID select product);
        else
        {
            List<Product> newUserFav = new (from product in _products where product.ID == productID select product);
            _favourites.Add(userName, newUserFav);
        }
        WriteFavourites(_favourites);
    }

    public async static Task DeleteFromFavouritesAsync(string userName, int productId)
    {
        await Task.Delay(10);
        if (_favourites.ContainsKey(userName))
            _favourites[userName].Remove((from product in _products where product.ID == productId select product).ElementAt(0));
        WriteFavourites(_favourites);
    }

    public async static Task<bool> IsProductFavouriteForUserAsync(string userName, int productId)
    {
        await Task.Delay(10);
        if (!_favourites.ContainsKey(userName)) return false;
        return (from product in _favourites[userName] where product.ID == productId select product).Any();
    }
}
