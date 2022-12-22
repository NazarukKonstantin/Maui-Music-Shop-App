namespace COURSE_ASH.Services;

public class CatalogService
{
    public async Task<IEnumerable<CatalogItem>> GetCategoriesAsync()
    {
        return await DataStorageService<CatalogItem>.GetItemListAsync();
    }
    public async Task<CatalogItem> GetCategoryInfo(string categoryName)
    {
        return await DataStorageService<CatalogItem>
            .GetItemByAsync(nameof(CatalogItem.Category), categoryName);
    }

    public async Task<bool> AddCategory(string name, FileResult categoryImage, double imageRotation, double imageScale)
    {
        if ((from categ in (await GetCategoriesAsync())
             where categ.Category == name select categ).Any())
            return false;

        CatalogItem category = new()
        {
            Category = name,
            ImageLink = await ImageManager<CatalogItem>.LinkImageToStorageAsync(categoryImage),
            ImageRotation = imageRotation,
            ImageScale = imageScale,
        };

        await DataStorageService<CatalogItem>.AddItemAsync(category);
        CategoryChanged?.Invoke(this, new CategoryEventArgs(category, CategoryEventArgs.CategoryWas.Added));

        return true;
    }

    public async Task ChangeCategoryAsync(string oldName, CatalogItem newItem, FileResult newCategoryImage=null)
    {
        string newImageLink = null;
        if (newCategoryImage != null)
            newImageLink = await ImageManager<CatalogItem>.LinkImageToStorageAsync(newCategoryImage);

        CatalogItem oldCategory = await DataStorageService<CatalogItem>.GetItemByAsync(nameof(CatalogItem.Category), oldName);

        if (oldName != newItem.Category)
            oldCategory.Category = newItem.Category;
        if (newImageLink != null)
        {
            if (await ImageManager<CatalogItem>.CountLinksAsync(oldCategory.ImageLink) <= 1)
                await ImageManager<CatalogItem>.RemoveImageAsync(oldCategory.ImageLink);
            oldCategory.ImageLink = newImageLink;
        }

        IEnumerable<Product> products =
            await DataStorageService<Product>.GetItemsByAsync(nameof(Product.Category), oldCategory.Category);
        foreach (Product product in products)
            await DataStorageService<Product>.UpdateItemAsync(product.CloneProductWithCategory(newItem.Category), 
                nameof(Product.ID), product.ID);

        await DataStorageService<CatalogItem>.UpdateItemAsync(oldCategory, nameof(CatalogItem.Category), oldName);
        CategoryChanged?.Invoke(this, new CategoryEventArgs(newItem, CategoryEventArgs.CategoryWas.Changed));
    }

    public async Task RemoveCategoryAsync(CatalogItem item)
    {
        await DataStorageService<CatalogItem>.DeleteItemsAsync(nameof(CatalogItem.Category), item.Category);
        await DataStorageService<Product>.DeleteItemsAsync(nameof(Product.Category), item.Category);

        if (await ImageSeekingService.ShouldDelete<CatalogItem>(item.ImageLink))
            await ImageManager<CatalogItem>.RemoveImageAsync(item.ImageLink);

        CategoryChanged?.Invoke(this, new CategoryEventArgs(item, CategoryEventArgs.CategoryWas.Removed));
    }

    public async Task<CatalogItem> FindCategoryBy(string name)
    {
        return await DataStorageService<CatalogItem>.GetItemByAsync(nameof(CatalogItem.Category),name);
    }

    public event EventHandler<CategoryEventArgs> CategoryChanged;
}
