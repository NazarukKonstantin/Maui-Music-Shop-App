namespace COURSE_ASH.Services;

public class ProductsService
{
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
