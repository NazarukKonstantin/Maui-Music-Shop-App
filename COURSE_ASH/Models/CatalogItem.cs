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

    public static List<CatalogItem> CatalogList { get; } = new()
    {
            new("Guitars", Icons.CatalogGuitar, 10, 1.6),
            new("Brass", Icons.CatalogSax, -60, 2),
            new("Accessories", Icons.CatalogGuitarAccessories, 0, 1.5),
            new("Strings", Icons.CatalogViolin, 18, 1.8),
            new("Ukuleles", Icons.CatalogUkulele, 0, 1.9),
            new("Keyboards", Icons.CatalogPiano, -10, 1)
    };

}
