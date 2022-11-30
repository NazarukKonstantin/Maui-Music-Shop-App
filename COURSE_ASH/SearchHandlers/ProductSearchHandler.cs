namespace COURSE_ASH.SearchHandlers;

public class ProductSearchHandler : SearchHandler
{
    public IList<Product> Products { get; set; }
    protected override void OnQueryChanged(string oldValue, string newValue)
    {
        base.OnQueryChanged(oldValue, newValue);

        if (string.IsNullOrEmpty(newValue))
        {
            ItemsSource = null;
        }
        else
        {
            ItemsSource = (from product in Products
                           where product.Model.ToLower().Contains(newValue.ToLower())
                           select product).ToList();
        }
    }
    protected override async void OnItemSelected(object item)
    {
        Product pickedProduct = item as Product;

        await Shell.Current.GoToAsync($"{nameof(ProductPageViewModel)}",
            new Dictionary<string, object>
            {
                ["PickedProduct"] = pickedProduct,
            });
    }

    protected override void OnQueryConfirmed()
    {
        if (string.IsNullOrEmpty(Query)) return;
        ObservableCollection<Product> searchResults = new((IEnumerable<Product>)ItemsSource);
        Shell.Current.GoToAsync($"{nameof(SearchPage)}",
        new Dictionary<string, object>
        {
            ["Title"] = Query,
            ["Products"] = searchResults
        });

    }
}
