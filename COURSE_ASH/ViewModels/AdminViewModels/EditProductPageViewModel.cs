namespace COURSE_ASH.ViewModels.AdminViewModels;

[QueryProperty(nameof(CurrentProduct), nameof(CurrentProduct))]
public partial class EditProductPageViewModel : BaseViewModel
{
    private FileResult _image = null;
    private readonly ProductsService _productsService;
    private readonly CatalogService _catalogService;

    [ObservableProperty]
    private ObservableCollection<string> _categories;

    [ObservableProperty]
    private Product _currentProduct;

    [ObservableProperty]
    private string _productType;

    [ObservableProperty]
    private string _productCategory;

    [ObservableProperty]
    private string _model;

    [ObservableProperty]
    private double _price;

    [ObservableProperty]
    private string _info;

    [ObservableProperty]
    private string _imageLink;

    public EditProductPageViewModel(ProductsService productsService, CatalogService catalogService)
    {
        _productsService = productsService;
        PropertyChanged += ProductChanged;
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
        Categories = (from cat 
                      in await _catalogService.GetCategoriesAsync() 
                      select cat.Category).ToObservableCollection();
    }
    private void ProductChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(CurrentProduct)) return;
        ProductType = CurrentProduct.ProductType;
        ProductCategory = CurrentProduct.Category;
        Model = CurrentProduct.Model;
        Price = CurrentProduct.Price;
        Info = CurrentProduct.Info;
        ImageLink = CurrentProduct.ImageLink;
    }

    [RelayCommand]
    public async Task ChangeProduct()
    {
        IsBusy = true;
        CurrentProduct.ProductType= ProductType;
        CurrentProduct.Category= ProductCategory;
        CurrentProduct.Model= Model;
        CurrentProduct.Price = Price;
        CurrentProduct.Info= Info;
        CurrentProduct.ImageLink = ImageLink;

        await _productsService.ChangeProductAsync(CurrentProduct, _image);
        await Shell.Current.DisplayAlert("SUCCESSFUL!", "Product changed", "OK");
        await GoBackAsync();
        IsBusy = false;
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        _image = null;
        _categories=null;
        _currentProduct=null;
        _productType=string.Empty;
        _productCategory=string.Empty;
        _model=string.Empty;
        _price=0;
        _info=string.Empty;
        _imageLink=string.Empty;
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    public async Task PickImage()
    {
        _image = await MediaPicker.PickPhotoAsync();
        if (_image is null) return;
        ImageLink = _image.FullPath;
    }

    [RelayCommand]
    public async Task AddCategoryAsync()
    {
        IsBusy = true;
        await Shell.Current.GoToAsync($"{nameof(AddCategoryPage)}");
        IsBusy = false;
    }
}
