namespace COURSE_ASH.Services;

public class ImageSeekingService
{
    public static async Task<bool> ShouldDelete<T>(string imageURI) where T : IImageDisposable
    {
        var favTask = IsImageInFavourites(imageURI);
        var cartTask = IsImageInCart(imageURI);
        var prodTask = ImageManager<T>.CountLinksAsync(imageURI);

        var Tasks = new List<Task> { favTask, cartTask, prodTask };

        while (Tasks.Count > 0)
        {
            Task finishedTask = await Task.WhenAny(Tasks);
            if (finishedTask == favTask)
            {
                if (favTask.Result)
                {
                    return false;
                }
            }
            else if (finishedTask == cartTask)
            {
                if (cartTask.Result)
                {
                    return false;
                }
            }
            else if (finishedTask == prodTask)
            {
                if (prodTask.Result != 0)
                {
                    return false;
                }
            }
            Tasks.Remove(finishedTask);
        }
        return true;
    }
    private static async Task<bool> IsImageInFavourites(string imageLink)
    {
        IEnumerable<AccountData> accounts = await DataStorageService<AccountData>.GetItemListAsync();
        foreach (AccountData account in accounts)
        {
            var favourites = await DataStorageService<FavouritesService.Favourites>
                .GetItemByAsync(nameof(FavouritesService.Favourites.CurrentLogin), account.CurrentLogin);
            if (favourites is null || favourites.Products is null) continue;
            if ((from product in favourites.Products where product.ImageLink == imageLink select product).Any())
                return true;
        }
        return false;
    }
    private static async Task<bool> IsImageInCart(string imageLink)
    {
        IEnumerable<AccountData> users = await DataStorageService<AccountData>.GetItemListAsync();
        foreach (AccountData account in users)
        {
            var cart = await DataStorageService<Cart>
                .GetItemByAsync(account.CurrentLogin, nameof(Cart.CurrentLogin));
            if (cart is null || cart.Products is null) continue;
            if ((from product in cart.Products where product.ImageLink == imageLink select product).Any())
                return true;
        }
        return false;
    }
}

