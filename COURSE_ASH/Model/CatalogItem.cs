namespace COURSE_ASH.Model
{
    public class CatalogItem
    {
        public string ProductsType { get; set; }
        public string ItemImage { get; set; }
        public int ImageRotation { get; set; }
        public double ImageScale { get; set; }

        public CatalogItem(string type, string image, int imageRotation, double imageScale)
        {
            ProductsType=type;
            ItemImage=image;
            ImageRotation=imageRotation;
            ImageScale=imageScale;
        }
    }
}
