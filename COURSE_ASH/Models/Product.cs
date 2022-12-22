namespace COURSE_ASH.Models;

public enum FilterField
{
    ID, P_TYPE, MODEL, PRICE, IMAGE_LINK, RATING
};

public enum SortDirection
{
    ASC,DESC,
}

public class Product : IImageDisposable
{
    public int ID { get; set; }
    public string ImageLink { get; set; }

    public string Category { get; set; }
    public string ProductType { get; set; }
    public string Model { get; set; }
    public string Info { get; set; }
    public double Price { get; set; }
    public double Rating { get; set; }
    public List<Review> Reviews { get; set; }

    public Product() { }

    public Product(string category, string productType, string model, string info, double price, string imageLink, double rating)
    {
        Category=category;
        ProductType=productType;
        Model=model;
        Info=info;
        Price=price;
        ImageLink=imageLink;
        Rating=rating;
    }

    public static List<string> ProductTypes { get; set; } = new()
    {
        "Guitar","MIDI","Sax","Ukulele","Violin","Guitar Accessory",
    };

    public bool Equals(object obj, FilterField filter=FilterField.ID)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Product pr = (Product)obj;
            return filter switch
            {
                FilterField.ID => pr.ID==ID,
                FilterField.P_TYPE => pr.ProductType == ProductType,
                FilterField.MODEL => pr.Model == Model,
                FilterField.PRICE => pr.Price == Price,
                FilterField.IMAGE_LINK => pr.ImageLink == ImageLink,
                FilterField.RATING => pr.Rating == Rating,
                _ => base.Equals(obj)
            };
        }
    }
}
