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
        product.ID = await DataStorageService<Product>.GetIDAsync();

        await DataStorageService<Product>.AddItemAsync(product);

        ProductChanged?.Invoke(this, new ProductEventArgs(product, ProductEventArgs.ProductWas.ADDED));
    }
    public async Task DeleteProductAsync(Product product)
    {
        await DataStorageService<Product>.DeleteFileAsync(product.ImageLink);

        await DataStorageService<Product>.DeleteItemAsync(nameof(Product.ID), product.ID);

        ProductChanged?.Invoke(this, new ProductEventArgs(product, ProductEventArgs.ProductWas.REMOVED));
    }
    public async Task ChangeProductAsync(Product product, FileResult productImage)
    {
        if (productImage != null)
            product.ImageLink = await DataStorageService<Product>.LinkToStorageAsync(productImage);

        await DataStorageService<Product>.UpdateItemAsync(product,nameof(Product.ID), product.ID);

        ProductChanged?.Invoke(this, new ProductEventArgs(product, ProductEventArgs.ProductWas.CHANGED));
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
