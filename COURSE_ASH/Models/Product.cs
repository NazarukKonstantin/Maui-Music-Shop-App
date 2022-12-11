namespace COURSE_ASH.Models;

public enum FilterField
{
    ID,P_TYPE, MODEL, INFO, PRICE, P_IMAGE, RATING
};

public class Product : IDBItem, IImageDisposable
{
    public int ID { get; set; }

    public string Image { get; set; }
    public int ImageLinkCounter { get; set; } = 0;

    public string Category { get; set; }
    public string ProductType { get; set; }
    public string Model { get; set; }
    public string Info { get; set; }
    public double Price { get; set; }
    public int Rating { get; set; }

    public Product() { }

    public Product(string category, string productType, string model, string info, double price, string image, int rating)
    {
        Category = category;
        ProductType=productType;
        Model=model;
        Info=info;
        Price=price;
        Image=image;
        Rating=rating;
    }

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
                FilterField.INFO => pr.Info == Info,
                FilterField.PRICE => pr.Price == Price,
                FilterField.P_IMAGE => pr.Image == Image,
                FilterField.RATING => pr.Rating == Rating,
                _ => base.Equals(obj)
            };
        }
    }
}
