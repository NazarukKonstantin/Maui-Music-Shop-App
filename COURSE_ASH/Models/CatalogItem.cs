namespace COURSE_ASH.Models;

public class CatalogItem
{
    public string Category { get; }
    public ImageSource ItemImage { get; }
    public int ImageRotation { get; set; }
    public double ImageScale { get; set; }

    public CatalogItem(string category, string image, int imageRotation, double imageScale)
    {
        Category=category;
        ItemImage=ImageSource.FromFile(image);
        ImageRotation=imageRotation;
        ImageScale=imageScale;
    }
}
