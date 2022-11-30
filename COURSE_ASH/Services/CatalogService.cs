namespace COURSE_ASH.Services;

public class CatalogService
{
    public List<CatalogItem> GetItems()
    {
        List<CatalogItem> items = new()
        {
            new("Guitars", "guitar_catalog", 10, 1.6),
            new("Brass", "sax_catalog", -60, 2),
            new("Accessories", "guitar_accessories_catalog", 0, 1.5),
            new("Strings", "violin_catalog", 18, 1.8),
            new("Ukulele", "ukulele_catalog", 0, 1.9),
            new("Keyboards", "midi_catalog", -10, 1)
        };
        return items;
    }
}
