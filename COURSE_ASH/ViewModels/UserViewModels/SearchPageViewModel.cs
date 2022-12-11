namespace COURSE_ASH.ViewModels.UserViewModels;

[QueryProperty(nameof(Products), nameof(Products))]
[QueryProperty(nameof(IsTabVisible), nameof(IsTabVisible))]
public partial class SearchPageViewModel : BaseViewModel
{
    [ObservableProperty]
    ObservableCollection<Product> _products;

    readonly ProductsService _prodService;

    private static List<Product> _cacheList;

    [ObservableProperty]
    string query;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsTabNotVisible))]
    private bool _isTabVisible;
    public bool IsTabNotVisible => !IsTabVisible;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotEmpty))]
    bool isEmpty = true;
    public bool IsNotEmpty => !isEmpty;

    public SearchPageViewModel(ProductsService service)
    {
        _prodService = service;
        _cacheList = _prodService.GetProducts();
        Products = _cacheList.ToObservableCollection();
        IsTabVisible = true;
        PropertyChanged += ProdCollectionChanged;
    }

    public void ProdCollectionChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(Products)) return;
        if (Products.Count == 0) IsEmpty = true;
        else IsEmpty = false;
    }

    public void SortProducts(FilterField filter = FilterField.MODEL)
    {
        switch (filter)
        {
            case FilterField.PRICE : Products.SortBy(pr=>pr.Price); break;
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


    [RelayCommand]
    private void FilterProducts ()
    {
        //switch (filter)
        //{
        //    case FilterField.PRICE: Products.SortBy(pr => pr.Price); break;
        //    case FilterField.P_TYPE: Products.FilterBy(pr => pr.ProductType); break;
        //    case FilterField.RATING: Products.FilterBy(pr => pr.Rating); break;
        //    default: Products.SortBy(pr => pr.Model); break;
        //};

        if (string.IsNullOrWhiteSpace(Query))
        {
            Query = string.Empty;
        }

        var filteredItems = _cacheList
            .Where(value => value.Model
            .ToLowerInvariant()
            .Contains(Query
            .ToLowerInvariant()))
            .ToList();

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
