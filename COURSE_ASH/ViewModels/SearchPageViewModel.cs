namespace COURSE_ASH.ViewModels;

[QueryProperty(nameof(Products), nameof(Products))]
[QueryProperty(nameof(IsTabVisible), nameof(IsTabVisible))]
public partial class SearchPageViewModel : BaseViewModel
{
    private readonly ProductsService _service;

    private static List<Product> _cacheList;

    public record struct RangeRecord(string LBorder, string RBorder);

    [ObservableProperty]
    private RangeRecord _range;

    [ObservableProperty]
    private int _rating;

    [ObservableProperty]
    ObservableCollection<Product> _products;

    [ObservableProperty]
    private string _query;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsTabNotVisible))]
    private bool _isTabVisible;
    public bool IsTabNotVisible => !IsTabVisible;

    [ObservableProperty]
    private bool _isFilterViewVisible;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotEmpty))]
    private bool _isEmpty = true;
    public bool IsNotEmpty => !IsEmpty;

    public SearchPageViewModel(ProductsService service)
    {
        _service = service;
        GetProducts();
        IsTabVisible = true;
        PropertyChanged += ProductCollectionChanged;
    }

    private async void GetProducts()
    {
        try
        {
            IsRefreshing = true;
            IsBusy = true;
            _cacheList = await _service.GetProductsAsync();
            Products = _cacheList.ToObservableCollection();
        }
        catch (Exception)
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not load products!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
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

    [RelayCommand]
    private void SortProductsBy(FilterField filter = FilterField.MODEL)
    {
        SortProductsBy(filter, SortDirection.ASC);
    }

    [RelayCommand]
    private void SortProductsByDescending(FilterField filter = FilterField.MODEL)
    {
        SortProductsBy(filter, SortDirection.DESC);
    }

    [RelayCommand]
    private void FilterProductsBy()
    {
        if (string.IsNullOrWhiteSpace(Query))
        {
            Query = string.Empty;
        }

        var filteredItems = _cacheList
                        .Where(value => value.Model
                        .ToLowerInvariant()
                        .Contains(Query
                        .ToLowerInvariant()));

        foreach (var value in _cacheList)
        {
            if (!filteredItems.Contains(value))
            {
                Products.Remove(value);
            }
            else if (!Products.Contains(value))
            {
                Products.Add(value);
                SortProductsBy(FilterField.MODEL, SortDirection.ASC);
            }
        }
    }

    //private IEnumerable<Product> GetEnumerableFilteredBy(FilterField filter = FilterField.MODEL)
    //{
    //    return filter switch
    //    {
    //        FilterField.PRICE => FilterByPrice(),
    //        FilterField.RATING => FilterByRating(),
    //        FilterField.P_TYPE => FilterByType(),
    //        _ => _cacheList
    //                    .Where(value => value.Model
    //                    .ToLowerInvariant()
    //                    .Contains(Query
    //                    .ToLowerInvariant())),
    //    };
    //}

    //private IEnumerable<Product> FilterByType()
    //{
    //    if (PickedTypes.Count==Product.ProductTypes.Count)
    //        return _cacheList;
    //    List<IEnumerable<Product>> tempList = new();
    //    foreach(var prod in PickedTypes)
    //    {
    //        tempList=(tempList.Append(_cacheList
    //                    .Where(value => value.ProductType
    //                    .ToLowerInvariant()
    //                    .Contains(prod.ProductType
    //                    .ToLowerInvariant()))))
    //                    .ToList();
    //    }
    //    return tempList.Cast<Product>();
    //}

    //private IEnumerable<ProdType> GetTypeList()
    //{
    //    List<string> temp = Product.ProductTypes;
    //    List<ProdType> tempList = new ();
    //    foreach (var type in temp)
    //    {
    //        tempList.Add(new ProdType
    //        {
    //            ProductType = type,
    //        });
    //    }
    //    return tempList;
    //}

    private IEnumerable<Product> FilterByPrice()
    {
        bool leftBorderExists = double.TryParse(Range.LBorder, out double lBorder)&&lBorder>=0;
        bool rightBorderExists = double.TryParse(Range.RBorder, out double rBorder)&&rBorder>=0;

        if (leftBorderExists&&rightBorderExists)
            return _cacheList
                .Where(p => (p.Price>=lBorder&&p.Price<=rBorder));
        else if (leftBorderExists)
            return _cacheList
                .Where(p => p.Price>=lBorder);
        else if (rightBorderExists)
            return _cacheList
                .Where(p => p.Price<=rBorder);
        else return _cacheList;
    }

    private IEnumerable<Product> FilterByRating()
    {
        if (Rating>GlobalConsts.MIN_RATING && Rating<=GlobalConsts.MAX_RATING)
            return _cacheList
                .Where(p => p.Rating==Rating);
        else return _cacheList;
    }


    private void SortProductsBy(FilterField filter = FilterField.MODEL, SortDirection direction = SortDirection.ASC)
    {
        switch (filter)
        {
            case FilterField.PRICE:
                switch (direction)
                {
                    case SortDirection.ASC:
                        Products=Products.OrderBy(pr => pr.Price)
                            .ToObservableCollection(); break;
                    case SortDirection.DESC:
                        Products=Products.OrderByDescending(pr => pr.Price)
                            .ToObservableCollection(); break;
                }
                break;

            case FilterField.RATING:
                switch (direction)
                {
                    case SortDirection.ASC:
                        Products = Products.OrderBy(pr => pr.Rating)
                    .ToObservableCollection(); break;
                    case SortDirection.DESC:
                        Products=Products.OrderByDescending(pr => pr.Rating)
                            .ToObservableCollection(); break;
                }
                break;

            case FilterField.P_TYPE:
                Products = Products.OrderBy(pr => pr.ProductType)
                    .ToObservableCollection(); break;
            default:
                Products = Products.OrderBy(pr => pr.Model)
                    .ToObservableCollection(); break;
        };
    }

    public void ProductCollectionChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(Products)) return;
        IsEmpty=Products.Count==0;
    }
}
