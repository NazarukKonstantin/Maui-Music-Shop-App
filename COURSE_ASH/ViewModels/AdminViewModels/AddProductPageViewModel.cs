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
    private string _category;

    [ObservableProperty]
    private string _model;

    [ObservableProperty]
    private string _price;

    [ObservableProperty]
    private string _info;

    [ObservableProperty]
    private string _imageLink;

    [ObservableProperty]
    private bool _isNotEmpty = false;

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
        Categories = (from cat in await _catalogService.GetCategoriesAsync() select cat.Category).ToObservableCollection();
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
        IsBusy = true;
        Product newProduct = new()
        {
            ProductType = _productType,
            Category = _category,
            Model = _model,
            Price = double.Parse(_price),
            Info = _info,
        };
        await _productsService.AddProductAsync(newProduct, _image);
        await Shell.Current.DisplayAlert("SUCCESSFUL!", "Product added", "OK");
        await GoBackAsync();
        IsBusy = false;
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        _image=null;
        _productType=string.Empty;
        _category=string.Empty;
        _model=string.Empty;
        _price=string.Empty;
        _info=string.Empty;
        _imageLink=null;
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
