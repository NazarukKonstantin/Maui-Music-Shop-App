using Firebase.Database.Query;

namespace COURSE_ASH.Services;

public class ProductsService : DBManager<Product>
{
    private FirebaseClient _client;

    public ProductsService(string url, string secret)
    {
        _client = BaseConstructor(url, secret);
    }

    public override async Task<bool> Add(Product value)
    {
        var response = await _client.Child(nameof(Product)).PostAsync(new Product()
        {
            ProductType=value.ProductType,
            Price=value.Price,
            ProductImage=value.ProductImage,
            Info = value.Info,
            Model = value.Model,
            Rating = value.Rating, 
        });
        return response.Key!=null;
    }

    public override async Task<bool> Delete(string Key)
    {
        throw new NotImplementedException();
    }

    public override async Task<Product> Get(string Key)
    {
        throw new NotImplementedException();
    }

    public override async Task<List<Product>> GetAll()
    {
        throw new NotImplementedException();
    }

    public override async Task<bool> Update(Product value)
    {
        if(!string.IsNullOrEmpty(value.Key))
            try
            {
                await _client.Child(nameof(Product)).Child(nameof(Product.Key)).PutAsync(value)
            }
    }

    public List<Product> GetProducts()
    {
        List<Product> products = new()
        {
            new("Guitar", "lol_1", "Good guitar", 132.42M, "guitar_catalog.png", 3),
            new("MIDI", "Model_1", "Good midi", 164.3423M, "midi_catalog.png", 5),
            new("Sax", "lol_2", "Good sax", 562.42M, "sax_catalog.png", 2),
            new("Ukulele", "Model_1", "Good ukulele", 32.42M, "ukulele_catalog.png", 1),
            new("Violin", "lol_3", "Good violin", 322.42M, "violin_catalog.png", 4),
            new("Guitar Accessory", "Model_1", "Good shit", 232.42M, "guitar_accessories_catalog.png", 0)
        };
        return products;
    }

    public List<Product> GetProducts(Product filterProduct, FilterField field=FilterField.P_TYPE)
    {
        return TempServer.FilterProducts(filterProduct,field);
    }

    public List<Product> GetProducts<T>(T filter, FilterField field = FilterField.P_TYPE)
    {
        return TempServer.FilterProducts(field,filter);
    }
}
