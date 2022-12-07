using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COURSE_ASH.Services;

public class CartService
{
    public List<Product> GetCartList()
    {
        return Cart.GetCartList();
    }
    public async Task<List<Product>> AddProduct(Product product)
    {
        List<Product> tempList = await Cart.AddProduct(product);
        CartChanged?.Invoke();
        return tempList;
    }
    public async Task<List<Product>> RemoveProduct(Product product)
    {
        List<Product> tempList = await Cart.RemoveProduct(product);
        CartChanged.Invoke();
        return tempList;
    }
    public void ClearCart()
    {
        Cart.ClearCart();
        CartChanged?.Invoke();
    }
    public async Task<int> CountProductInCart(int productId)
    {
        return await Cart.CountProductInCart(productId);
    }

    public event CartUpdated CartChanged;

    public delegate void CartUpdated();
}
