namespace COURSE_ASH.Services;

public class CatalogService
{
    public async Task<IEnumerable<CatalogItem>> GetCategoriesAsync()
    {
        return await DataStorageService<CatalogItem>.GetItemListAsync();
    }
    public async Task<bool> AddCategory(string name, FileResult categoryImage, double imageRotation, double imageScale)
    {
        if ((from categ in (await GetCategoriesAsync())
             where categ.Category == name select categ).Any())
            return false;

        CatalogItem category = new()
        {
            Category = name,
            ImageLink = await ImageService<CatalogItem>.LinkImageToStorageAsync(categoryImage),
            ImageRotation = imageRotation,
            ImageScale = imageScale,
        };

        await DataStorageService<CatalogItem>.AddItemAsync(category);
        CategoryChanged?.Invoke(this, new CategoryEventArgs(category, CategoryEventArgs.CategoryWas.Added));

        return true;
    }

    public async Task ChangeCategoryAsync(string oldName, CatalogItem newItem, ProductsService productsService, FileResult newCategoryImage=null)
    {
        if (newCategoryImage != null)
        {
            await DisposeImageOfAsync(newItem);
            newItem.ImageLink = await ImageService<Product>.LinkImageToStorageAsync(newCategoryImage);
        }
        if (!newItem.Category.Equals(oldName))
        {
            List<Product> newProds = (await DataStorageService<Product>
                .GetItemsByAsync(nameof(Product.Category), oldName)).ToList();
            foreach (var prod in newProds)
            {
                prod.Category=newItem.Category;
                await productsService.ChangeProductAsync(prod);
            }
            await DataStorageService<CatalogItem>.DeleteItemAsync(nameof(CatalogItem.Category), oldName);
            await DataStorageService<CatalogItem>.AddItemAsync(newItem);
        }
        else
        {
            await DataStorageService<CatalogItem>
            .UpdateItemAsync(newItem, nameof(CatalogItem.Category), newItem.Category);
        }

        CategoryChanged?.Invoke(this, new CategoryEventArgs(newItem, CategoryEventArgs.CategoryWas.Changed));
    }
    public async Task RemoveCategoryAsync(string name, ProductsService service)
    {
        CatalogItem item = await FindCategoryBy(name);
        await RemoveCategoryAsync(item,service);
    }

    public async Task RemoveCategoryAsync(CatalogItem item,ProductsService service)
    {
        await DisposeImageOfAsync(item);
        List<Product> newProds = (await DataStorageService<Product>
                .GetItemsByAsync(nameof(Product.Category), item.Category)).ToList();
        foreach (var prod in newProds)
        {
            await service.DeleteProductAsync(prod);
        }
        await DataStorageService<CatalogItem>.DeleteItemAsync(nameof(CatalogItem.Category), item.Category);

        CategoryChanged?.Invoke(this, new CategoryEventArgs(item, CategoryEventArgs.CategoryWas.Removed));
    }

    public async Task<CatalogItem> FindCategoryBy(string name)
    {
        return await DataStorageService<CatalogItem>.GetItemByAsync(nameof(CatalogItem.Category),name);
    }

    private async Task DisposeImageOfAsync(CatalogItem item)
    {
        if ((await ImageService<CatalogItem>.CountLinksAsync(item.ImageLink))==1)
            await ImageService<Product>.RemoveImageAsync(item.ImageLink);
    }

    public event EventHandler<CategoryEventArgs> CategoryChanged;
}
