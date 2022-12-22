namespace COURSE_ASH.ViewModels.AdminViewModels;

public partial class AddProductPageViewModel : BaseViewModel
{
    private FileResult _image;
    private readonly ProductsService _productsService;
    private readonly CatalogService _catalogService;

    [ObservableProperty]
    private ObservableCollection<string> _categories;

    [ObservableProperty]
    private string _productType;

    [ObservableProperty]
    string _category;

    [ObservableProperty]
    string _model;

    [ObservableProperty]
    string _price;

    [ObservableProperty]
    string _info;

    [ObservableProperty]
    string _imageLink;

    [ObservableProperty]
    bool _isNotEmpty;

    public AddProductPageViewModel(ProductsService productsService, CatalogService catalogService)
    {
        _productsService = productsService;
        PropertyChanged += CheckAnyFieldEmpty;
        _catalogService=catalogService;
        _catalogService.CategoryChanged+=CategoryChanged;
        GetCategories();
    }

    private void CategoryChanged(object sender, CategoryEventArgs e)
    {
        GetCategories();
    }
    public async void GetCategories()
    {
        try
        {
            IsBusy=true;
            IsRefreshing = true;
            Categories = (from cat 
                          in await _catalogService.GetCategoriesAsync() 
                          select cat.Category).ToObservableCollection();
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("ERROR", "Could not load categories!", "OK");
        }
        finally
        {
            IsRefreshing=false;
            IsBusy=false;
        }
    }

    [RelayCommand]
    public async Task PickImage()
    {
        _image = await MediaPicker.PickPhotoAsync();
        if (_image is null) return;
        ImageLink = _image.FullPath;
    }

    private void CheckAnyFieldEmpty(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IsNotEmpty)) return;
        IsNotEmpty = !(string.IsNullOrEmpty(ProductType) ||
            string.IsNullOrEmpty(Category) ||
            string.IsNullOrEmpty(Model) ||
            string.IsNullOrEmpty(Info) ||
            string.IsNullOrEmpty(Price) ||
            string.IsNullOrEmpty(ImageLink));
    }

    [RelayCommand]
    public async Task AddProduct()
    {
        try
        {
            IsBusy = true;
            Product newProduct = new()
            {
                ProductType = ProductType,
                Category = Category,
                Model = Model,
                Price = double.Parse(Price),
                Info = Info,
            };
            await _productsService.AddProductAsync(newProduct, _image);
            await Shell.Current.DisplayAlert("SUCCESSFUL!", "Product added", "OK");
            await GoBackAsync();
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("ERROR", "Could not add product!", "OK");
        }
        finally
        {
            IsBusy=false;
        }
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        _image=null;
        ProductType=string.Empty;
        Category=string.Empty;
        Model=string.Empty;
        Price=string.Empty;
        Info=string.Empty;
        ImageLink=null;
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    public async Task AddCategoryAsync()
    {
        IsBusy = true;
        await Shell.Current.GoToAsync($"{nameof(AddCategoryPage)}");
        IsBusy = false;
    }
}
