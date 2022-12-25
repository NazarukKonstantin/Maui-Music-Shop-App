using COURSE_ASH.Models;
using COURSE_ASH.Models.Interfaces;
using COURSE_ASH.Views;
using Microsoft.Maui;

namespace COURSE_ASH.Services;

public class ProductsService
{
    public async Task<List<Product>> GetProductsAsync()
    {
        return (await DataStorageService<Product>.GetItemListAsync()).ToList();
    }

    public async Task AddProductAsync(Product product, FileResult productImage)
    {
        product.ImageLink = await ImageManager<Product>.LinkImageToStorageAsync(productImage);
        var products = await GetProductsAsync() ?? new List<Product>();
        int newId = products.Count == 0 ? 1 : (from prod in products select prod.ID).Max() + 1;
        product.ID = newId;

        await DataStorageService<Product>.AddItemAsync(product);

        ProductChanged?.Invoke(this, new ProductEventArgs(product, ProductEventArgs.ProductWas.Added));
    }
    public async Task DeleteProductAsync(Product product)
    {
        await DataStorageService<Product>.DeleteItemsAsync(nameof(Product.ID), product.ID);

        if (await ImageSeekingService.ShouldDelete<Product>(product.ImageLink))
            await ImageManager<Product>.RemoveImageAsync(product.ImageLink);

        ProductChanged?.Invoke(this, new ProductEventArgs(product, ProductEventArgs.ProductWas.Removed));
    }
    public async Task ChangeProductAsync(Product product, FileResult productImage=null)
    {
        string oldImageLink = product.ImageLink;
        if (productImage != null)
            product.ImageLink = await ImageManager<Product>.LinkImageToStorageAsync(productImage);

        await DataStorageService<Product>.UpdateItemAsync(product, nameof(Product.ID), product.ID);

        if (productImage != null && await ImageSeekingService.ShouldDelete<Product>(oldImageLink))
            await ImageManager<Product>.RemoveImageAsync(oldImageLink);

        ProductChanged?.Invoke(this, new ProductEventArgs(product, ProductEventArgs.ProductWas.Changed));
    }

    public async Task<List<Review>> GetReviewsForAsync(Product product)
    {
        Product prod = await GetByIDAsync(product);
        if (prod is null)
        {
            await Shell.Current.DisplayAlert("ERROR!", $"{product.Model} doesn't exist", "OK");
        }
        return prod?.Reviews ?? new List<Review>();
    }

    public async Task AddReviewForAsync(Product product, Review review)
    {
        Product prod = await GetByIDAsync(product);
        if (prod is null)
        {
            await Shell.Current.DisplayAlert("ERROR!", $"{product.Model} doesn't exist", "OK");
        }
        if (review != null)
        {
            review.CurrentLogin = App.CurrentLogin;
            product.Reviews.Add(review);
        }

        await UpdateByIDAsync(product);

        ProductChanged?.Invoke(this, new ProductEventArgs(product, ProductEventArgs.ProductWas.Changed));
    }

    public async Task ChangeReviewForAsync(Product product, Review prev_review,Review new_review)
    {
        Product prod = await GetByIDAsync(product);
        if (prod is null)
        {
            await Shell.Current.DisplayAlert("ERROR!", $"{product.Model} doesn't exist", "OK");
        }
        if (new_review != null && prev_review!=null)
        {
            product.Reviews[product.Reviews.IndexOf(prev_review)]=new_review;
        }
        await UpdateByIDAsync(product);
        ProductChanged?.Invoke(this, new ProductEventArgs(product, ProductEventArgs.ProductWas.Changed));
    }

    private async Task<Product> GetByIDAsync(Product product)
    {
       return await DataStorageService<Product>.GetItemByAsync(nameof(Product.ID), product.ID);
    }
    private async Task UpdateByIDAsync(Product product)
    {
        await DataStorageService<Product>.UpdateItemAsync(product, nameof(Product.ID), product.ID);
    }

    public void CountRatingOf(Product product)
    {
        product.Rating=0;
        int ratingSum = 0;
        foreach(var review in product.Reviews)
        {
            ratingSum+=review.Rating;
        }
        product.Rating=ratingSum/(double)product.Reviews.Count;
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
