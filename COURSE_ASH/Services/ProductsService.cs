using COURSE_ASH.Models;
using COURSE_ASH.Models.Interfaces;

namespace COURSE_ASH.Services;

public class ProductsService
{
    public async Task<List<Product>> GetProductsAsync()
    {
        return (await DataStorageService<Product>.GetItemListAsync()).ToList();
    }
    public async Task AddProductAsync(Product product, FileResult productImage)
    {
        product.ImageLink = await DataStorageService<Product>.LinkToStorageAsync(productImage);
        var products = await GetProductsAsync();
        int newId = (from p in products select p.ID)?.Max() + 1 ?? 1;
        product.ID = newId;

        await DataStorageService<Product>.AddItemAsync(product);

        ProductChanged?.Invoke(this, new ProductEventArgs(product, ProductEventArgs.ProductWas.Added));
    }
    public async Task DeleteProductAsync(Product product)
    {
        await DisposeImageOfAsync(product);

        await DataStorageService<Product>.DeleteItemAsync(nameof(Product.ID), product.ID);

        ProductChanged?.Invoke(this, new ProductEventArgs(product, ProductEventArgs.ProductWas.Removed));
    }
    public async Task ChangeProductAsync(Product product, FileResult productImage=null)
    {
        if (productImage != null)
        {
            await DisposeImageOfAsync(product);
            product.ImageLink = await DataStorageService<Product>.LinkToStorageAsync(productImage);
        }

        await DataStorageService<Product>.UpdateItemAsync(product, nameof(Product.ID), product.ID);

        ProductChanged?.Invoke(this, new ProductEventArgs(product, ProductEventArgs.ProductWas.Changed));
    }

    private async Task DisposeImageOfAsync(Product product)
    {
        if ((await DataStorageService<Product>.Count(nameof(Product.ImageLink), product.ImageLink))==1)
            await DataStorageService<Product>.DeleteFileAsync(product.ImageLink);
    }

    public event EventHandler<ProductEventArgs> ProductChanged;

    public List<Product> GetProducts()
    {
        List<Product> products = new()
        {
            new("Guitars","Guitar", "guitar_1", "Good guitar", 132.42, Icons.CatalogGuitar, 3),
            new("Keyboards","MIDI", "midi_1", "Good midi", 164.3423, Icons.CatalogPiano, 5),
            new("Brass","Sax", "sax_2", "Good sax", 562.42, Icons.CatalogSax, 2),
            new("Ukuleles","Ukulele", "ukulele_1", "Good ukulele", 32.42, Icons.CatalogUkulele, 1),
            new("Strings","Violin", "violin_3", "Good violin", 322.42, Icons.CatalogViolin, 4),
            new("Accessories","Guitar Accessory", "Model_1", "Good shit", 232.42, Icons.CatalogGuitarAccessories, 0)
        };
        return products;
    }
}
