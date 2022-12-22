namespace COURSE_ASH.Models;

public class CartProduct : Product
{
    public int UnitQuantity { get; set; }

    public CartProduct() : base()
    {
    }

    public CartProduct(Product product) : base()
    {
        ID = product.ID;
        ProductType = product.ProductType;
        Category = product.Category;
        Price = product.Price;
        Rating= product.Rating;
        Info = product.Info;
        ImageLink = product.ImageLink;
        UnitQuantity = 0;
    }
    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }

        CartProduct pr = (CartProduct)obj;
        return pr.ID == ID;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public Product GetProduct()
    {
        return new Product
        {
            ID = ID,
            ProductType = ProductType,
            Category = Category,
            Price = Price,
            Rating = Rating,
            Info= Info,
            ImageLink=ImageLink,
        };
    }
}
