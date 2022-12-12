namespace COURSE_ASH.ViewModels.UserViewModels;

[QueryProperty(nameof(Products), nameof(Products))]
[QueryProperty(nameof(IsTabVisible), nameof(IsTabVisible))]
public partial class SearchPageViewModel : BaseViewModel
{
    private readonly ProductsService _service;

    private static List<Product> _cacheList;

    [ObservableProperty]
    ObservableCollection<Product> _products;

    [ObservableProperty]
    private string query;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsTabNotVisible))]
    private bool _isTabVisible;
    public bool IsTabNotVisible => !IsTabVisible;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotEmpty))]
    private bool _isEmpty = true;
    public bool IsNotEmpty => !IsEmpty;

    public SearchPageViewModel(ProductsService service)
    {
        _service = service;
        _cacheList = _service.GetProducts();
        Products = _cacheList.ToObservableCollection();
        IsTabVisible = true;
        PropertyChanged += ProductCollectionChanged;
    }

    public void ProductCollectionChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(Products)) return;
        IsEmpty=Products.Count==0;
    }

    public void SortProducts(FilterField filter = FilterField.MODEL)
    {
        switch (filter)
        {
            case FilterField.PRICE : Products.SortBy(pr => pr.Price); break;
            case FilterField.P_TYPE : Products.SortBy(pr => pr.ProductType); break;
            case FilterField.RATING : Products.SortBy(pr => pr.Rating); break;
            default: Products.SortBy(pr => pr.Model); break;
        };
    }

    //[RelayCommand]
    //private void SearchProduct()
    //{
    //    var model=string.Empty;
    //    FilterProducts(model => model.StartsWith(Query));
    //}
    private List<Product> GetListFilteredBy(FilterField filter=FilterField.MODEL)
    {
        return filter switch
        {
            FilterField.PRICE => _cacheList
                        .Where(value => value.Rating
                        .ToString()
                        .Contains(Query))
                        .ToList(),
            FilterField.P_TYPE => _cacheList
                        .Where(value => value.ProductType
                        .ToLowerInvariant()
                        .Contains(Query
                        .ToLowerInvariant()))
                        .ToList(),
            FilterField.RATING => _cacheList
                        .Where(value => value.Rating
                        .ToString()
                        .Contains(Query))
                        .ToList(),
            _ => _cacheList
                        .Where(value => value.Model
                        .ToLowerInvariant()
                        .Contains(Query
                        .ToLowerInvariant()))
                        .ToList(),
        };
    }


    [RelayCommand]
    private void FilterProducts (FilterField filter=FilterField.MODEL)
    {
        if (string.IsNullOrWhiteSpace(Query))
        {
            Query = string.Empty;
        }

        var filteredItems = GetListFilteredBy(filter);

        foreach (var value in _cacheList)
        {
            if (!filteredItems.Contains(value))
            {
                Products.Remove(value);
            }
            else if (!Products.Contains(value))
            {
                Products.Add(value);
                SortProducts();
            }
        }
    }

    [RelayCommand]
    async Task GoToProductPageAsync(Product product)
    {
        await Shell.Current.GoToAsync($"{nameof(ProductPage)}",
            new Dictionary<string, object>
            {
                ["PickedProduct"] = product
            });
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        IsTabVisible=true;
        await Shell.Current.GoToAsync("..");
    }
}
