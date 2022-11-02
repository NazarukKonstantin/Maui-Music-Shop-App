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
    }
}
