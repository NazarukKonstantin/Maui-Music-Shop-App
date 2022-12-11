namespace COURSE_ASH.Services;

public class FavouritesService
{
    public async Task<List<Product>> GetFavouritesForUserAsync(string currentLogin)
    {
        return await TempService.GetFavouritesForUserAsync(currentLogin);
    }
    public async Task SetFavouriteAsync(string currentLogin, int productId)
    {
        await TempService.SetFavouriteAsync(currentLogin, productId);
    }
    public async Task DeleteFromFavouritesAsync(string currentLogin, int productId)
    {
        await TempService.DeleteFromFavouritesAsync(currentLogin, productId);
    }
    public async Task<bool> IsProductFavouriteForUserAsync(string currentLogin, int productId)
    {
        return await TempService.IsProductFavouriteForUserAsync(currentLogin, productId);
    }
}
