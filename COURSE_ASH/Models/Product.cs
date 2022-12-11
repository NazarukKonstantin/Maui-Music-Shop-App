﻿namespace COURSE_ASH.Models;

public enum FilterField
{
    ID, P_TYPE, MODEL, PRICE, IMAGE_LINK, RATING
};

public class Product : IImageDisposable
{
    public int ID { get; set; }

    public string ImageLink { get; set; }
    public int ImageLinkCounter { get; set; } = 0;

    public string Category { get; set; }
    public string ProductType { get; set; }
    public string Model { get; set; }
    public string Info { get; set; }
    public double Price { get; set; }
    public int Rating { get; set; }

    public Product() { }

    public Product(string category, string productType, string model, string info, double price, string imageLink, int rating)
    {
        Category=category;
        ProductType=productType;
        Model=model;
        Info=info;
        Price=price;
        ImageLink=imageLink;
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
                FilterField.PRICE => pr.Price == Price,
                FilterField.IMAGE_LINK => pr.ImageLink == ImageLink,
                FilterField.RATING => pr.Rating == Rating,
                _ => base.Equals(obj)
            };
        }
    }
}
