
using System.Text.Json;

namespace COURSE_ASH.Models;

public class Cart
{
    static string _path = FileSystem.Current.CacheDirectory;
    static string _fullPath = Path.Combine(_path, "CartData.json");

    static List<CartProduct> _products;
    static List<CartProduct> ReadProducts()
    {
        List<CartProduct> products;
        using (FileStream fs = new(_fullPath, FileMode.OpenOrCreate))
        {
            try
            {
                products = JsonSerializer.Deserialize<List<CartProduct>>(fs);
            }
            catch (Exception)
            {
                products = new List<CartProduct>();
            }
        }
        return products;
    }
    static async Task WriteProducts(List<CartProduct> products)
    {
        using FileStream fs = new(_fullPath, FileMode.Create);
        await JsonSerializer.SerializeAsync<List<CartProduct>>(fs, products);
    }
    static Cart()
    {
        _products = ReadProducts();
    }
    public static List<CartProduct> GetCartList()
    {
        return _products;
    }
    public async static Task<List<CartProduct>> AddProduct(CartProduct product)
    {
        if (!_products.Contains(product))
        {
            product.UnitQuantity++;
            _products.Add(product);
        }
        else
        {
            _products[_products.IndexOf(product)].UnitQuantity++;
        }
        await WriteProducts(_products);
        return _products;
    }
    public async static Task<List<CartProduct>> RemoveProduct(CartProduct product)
    {
        await Task.Delay(1);
        CartProduct tempProd = _products.Find(pr => pr.ID == product.ID);
        if (tempProd.UnitQuantity != 1) tempProd.UnitQuantity--;
        else _products.Remove(tempProd);
        await WriteProducts(_products);
        return _products;
    }
    public async static void ClearCart()
    {
        _products.Clear();
        await WriteProducts(_products);
    }
    public async static Task<int> CountProductInCart(int productId)
    {
        await Task.Delay(1);
        if (!_products.Any()) return 0;
        if ((from product in _products where product.ID == productId select product).Count() == 0) return 0;
        return (from product in _products where product.ID == productId select product.UnitQuantity).ElementAt(0);
    }
}
