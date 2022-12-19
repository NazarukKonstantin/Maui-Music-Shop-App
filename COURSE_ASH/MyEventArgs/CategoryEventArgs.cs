namespace COURSE_ASH.MyEventArgs;

public class CategoryEventArgs : EventArgs
{
    public CategoryWas State { get; set; }
    public CatalogItem Item { get; set; }

    public CategoryEventArgs(CatalogItem item, CategoryWas newState)
    {
        Item = item;
        State = newState;
    }

    public enum CategoryWas
    {
        Added, Removed, Changed,
    }
}
