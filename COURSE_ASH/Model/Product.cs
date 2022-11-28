namespace COURSE_ASH.Model
{
    public class Product
    {
        public string ProductType { get; set; }
        public string Model { get; set; }
        public string Info { get; set; }
        public decimal Price { get; set; }
        public string ProductImage { get; set; }
        public int Rating { get; set; }

        public Product(string productType, string model, string info, decimal price, string productImage, int rating)
        {
            ProductType=productType;
            Model=model;
            Info=info;
            Price=price;
            ProductImage=productImage;
            Rating=rating;
        }

        public static enum FilterField
        {
            P_TYPE,MODEL,INFO,PRICE,P_IMAGE,RATING
        };

        public bool Equals(object obj, FilterField filter)
        {
            Product pr=(Product)obj;
            return filter switch
            {
                FilterField.P_TYPE => pr.ProductType == ProductType,
                FilterField.MODEL => pr.Model == Model,
                FilterField.INFO => pr.Info == Info,
                FilterField.PRICE => pr.Price == Price,
                FilterField.P_IMAGE => pr.ProductImage == ProductImage,
                FilterField.RATING => pr.Rating == Rating,
                _ => base.Equals(obj)
            };
        }
    }
}
