namespace COURSE_ASH.MyEventArgs;

public class ProductEventArgs : EventArgs
{
    public ProductWas State { get; set; }
    public Product Product { get; set; }

    public ProductEventArgs(Product product, ProductWas newState)
    {
        Product = product;
        State = newState;
    }

    public enum ProductWas
    {
        ADDED,REMOVED,CHANGED,
    }
}
