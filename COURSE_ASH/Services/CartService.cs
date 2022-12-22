using Microsoft.Maui;

namespace COURSE_ASH.Services;

public class CartService
{
    public async Task<List<CartProduct>> GetCartFromStorageAsync(string currentLogin)
    {
        Cart cart = await GetCartBy(currentLogin);

        return cart ?.Products ?? new List<CartProduct>();
    }
    public async Task<CartProduct> AddProductToStorageCartAsync(string currentLogin, CartProduct product)
    {
        product.UnitQuantity++;

        Cart cart = await GetCartBy(currentLogin);

        if (cart is null)
        {
            await DataStorageService<Cart>.AddItemAsync(new Cart
            {
                CurrentLogin = currentLogin,
                Products = new List<CartProduct> { product },
            });
            return product;
        }

        cart.Products ??= new List<CartProduct>();

        if (cart.Products.Contains(product))
            cart.Products[cart.Products.IndexOf(product)].UnitQuantity++;
        else
            cart.Products.Add(product);

        await UpdateCartGlobal(cart);

        return product;
    }
    public async Task DeleteProductFromStorageCartAsync(string currentLogin, Product product)
    {
        Cart cart = await GetCartBy(currentLogin);

        if (cart is not null)
        {
            cart.Products.Remove(new CartProduct(product));

            await UpdateCartGlobal(cart);

            if (await ImageSeekingService.ShouldDelete<Product>(product.ImageLink))
                await ImageManager<Product>.RemoveImageAsync(product.ImageLink);
        }
    }
    public async Task<CartProduct> RemoveProductFromStorageCartAsync(string currentLogin,CartProduct product)
    {
        product.UnitQuantity--;

        Cart cart = await GetCartBy(currentLogin);

        if (product.UnitQuantity == 0)
            cart.Products.Remove(product);
        else
            cart.Products[
                cart.Products.IndexOf(product)].UnitQuantity--;

        await UpdateCartGlobal(cart);

        if (await ImageSeekingService.ShouldDelete<Product>(product.ImageLink))
            await ImageManager<Product>.RemoveImageAsync(product.ImageLink);
        return product;
    }
    public async Task ClearStorageCartAsync(string currentLogin)
    {
        Cart cart = await GetCartBy(currentLogin);
        if(cart is not null)
        await DataStorageService<Cart>.DeleteItemsAsync
                (nameof(Cart.CurrentLogin),cart.CurrentLogin);

        foreach (Product product in cart.Products)
            if (await ImageSeekingService.ShouldDelete<Product>(product.ImageLink))
            await ImageManager<Product>.RemoveImageAsync(product.ImageLink);
    }

    public async Task<bool> IsProductInStorageCart(Product product, string currentLogin)
    {
        Cart cart = await GetCartBy(currentLogin);

        if (cart is null || cart.Products is null) return false;

        return (from cartProduct 
                in cart.Products 
                where cartProduct.ID == product.ID 
                select cartProduct).Any();
    }

    private async Task<Cart> GetCartBy(string currentLogin)
    {
        Cart cart = await DataStorageService<Cart>.GetItemByAsync(nameof(Cart.CurrentLogin), currentLogin);
        if (cart is null)
        {
            await Shell.Current.DisplayAlert("ERROR!", $"Cart of {currentLogin} doesn't exist", "OK");
        }
        return cart;
    }

    private async Task UpdateCartGlobal(Cart cart)
    {
       await DataStorageService<Cart>.UpdateItemAsync(cart, nameof(Cart.CurrentLogin), cart.CurrentLogin);
    }
}
