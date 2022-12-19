namespace COURSE_ASH.Services;

public class FavouritesService
{
    public record Favourites(string CurrentLogin)
    {
        public List<Product> Products { get; set; }
    };
    public async Task<List<Product>> GetFavouriteProductsAsync(string currentLogin)
    {
        return (await GetFavouritesByAsync(currentLogin))?.Products ?? new();
    }
    public async Task SetFavouriteAsync(string currentLogin, Product product)
    {
        Favourites favourites = await GetFavouritesByAsync(currentLogin);

        favourites ??= new (currentLogin);
        favourites.Products ??= new();
        favourites.Products.Add(product);

        await UpdateFavouritesByAsync(currentLogin, favourites);
    }
    public async Task DeleteFromFavouritesAsync(string currentLogin, Product product)
    {
        Favourites favourites = await GetFavouritesByAsync(currentLogin);
        favourites.Products.Remove(product);
        await UpdateFavouritesByAsync(currentLogin, favourites);
    }
    public async Task<bool> IsProductFavouriteAsync(string currentLogin, Product product)
    {
        Favourites favourites = await GetFavouritesByAsync(currentLogin);
        return favourites?.Products is not null &&
            favourites.Products.Contains(product);
    }

    private async Task<Favourites> GetFavouritesByAsync(string login)
    {
        return await DataStorageService<Favourites>.GetItemByAsync(nameof(Favourites.CurrentLogin), login);
    }

    private async Task UpdateFavouritesByAsync(string login, Favourites favourites)
    {
        await DataStorageService<Favourites>.UpdateItemAsync(favourites, nameof(Favourites.CurrentLogin), login);
    }
}
