namespace COURSE_ASH.Models;

public class CatalogItem : IImageDisposable
{
    public string Category { get; set; }
    public string ImageLink { get; set;  }
    public double ImageRotation { get; set; }
    public double ImageScale { get; set; }

    public CatalogItem () { }

    public CatalogItem(string category, string imageLink, double imageRotation, double imageScale)
    {
        Category=category;
        ImageLink=imageLink;
        ImageRotation=imageRotation;
        ImageScale=imageScale;
    }

    public static List<CatalogItem> CatalogList { get; } = new()
    {
            new("Guitars", Icons.CatalogGuitar, 10.0, 1.6),
            new("Brass", Icons.CatalogSax, -60.0, 2),
            new("Accessories", Icons.CatalogGuitarAccessories, 0, 1.5),
            new("Strings", Icons.CatalogViolin, 18.0, 1.8),
            new("Ukuleles", Icons.CatalogUkulele, 0, 1.9),
            new("Keyboards", Icons.CatalogPiano, -10.0, 1)
    };

}
