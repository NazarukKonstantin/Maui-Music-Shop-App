namespace COURSE_ASH.Services;

public class ProductsService
{
    private FirebaseClient _client = App.DBManager.Client;

    public async Task<bool> Add (Product value)
    {
        var response = await _client
            .Child(nameof(Product))
            .PostAsync(value);
        return response.Key!=null;
    }

    public async Task AddCollection(ObservableCollection<Product> values)
    {
        foreach(var value in values)
        {
            if (!(await Add(value)))
                Console.WriteLine("ERROR while adding in DB");
        }
    }

    public async Task<bool> Delete(string key)
    {
        if (!string.IsNullOrEmpty(key))
            try
            {
                var DeleteTarget = (await _client
                                    .Child(nameof(Product))
                                     .OnceAsync<Product>()).FirstOrDefault
                                      (a => a.Object.Key == key);
                await _client.Child(nameof(Product)).Child(DeleteTarget.Key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        else Console.WriteLine("No Key");
        return false;
    }

    public ObservableCollection<Product> GetAll()
    {
        var data = _client
                .Child(nameof(Product))
                .AsObservable<Product>()
                .AsObservableCollection();
        return data;
    }

    public async Task<bool> Update(Product new_value)
    {
        if (string.IsNullOrEmpty(new_value.Key))
        {
            try
            {
                var UpdateTarget = (await _client
                .Child(nameof(Product))
                .OnceAsync<Product>()).FirstOrDefault
                (a => a.Object.Key==new_value.Key);

                await _client
                    .Child(nameof(Product))
                    .Child(UpdateTarget.Key)
                    .PutAsync(new_value);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        else Console.WriteLine("No Key");
        return false;
    }

    public List<Product> GetProducts()
    {
        List<Product> products = new()
        {
            new("Guitars","Guitar", "guitar_1", "Good guitar", 132.42, "guitar_catalog.png", 3),
            new("Keyboards","MIDI", "midi_1", "Good midi", 164.3423, "midi_catalog.png", 5),
            new("Brass","Sax", "sax_2", "Good sax", 562.42, "sax_catalog.png", 2),
            new("Ukuleles","Ukulele", "ukulele_1", "Good ukulele", 32.42, "ukulele_catalog.png", 1),
            new("Strings","Violin", "violin_3", "Good violin", 322.42, "violin_catalog.png", 4),
            new("Accessories","Guitar Accessory", "Model_1", "Good shit", 232.42, "guitar_accessories_catalog.png", 0)
        };
        return products;
    }

    public List<Product> GetProducts(Product filterProduct, FilterField field = FilterField.P_TYPE)
    {
        return TempServer.FilterProducts(filterProduct, field);
    }

    public List<Product> GetProducts<T>(T filter, FilterField field = FilterField.P_TYPE)
    {
        return TempServer.FilterProducts(field, filter);
    }
}
