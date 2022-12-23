namespace COURSE_ASH.ViewModels.AdminViewModels;

[QueryProperty(nameof(CurrentProduct), nameof(CurrentProduct))]
public partial class EditProductPageViewModel : BaseViewModel
{
    private FileResult _image = null;
    private string _oldImage = null;
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
    private string _price;

    [ObservableProperty]
    private string _info;

    [ObservableProperty]
    private string _imageLink;


    [ObservableProperty]
    bool isNotEmpty = false;


    public EditProductPageViewModel(ProductsService productsService, CatalogService catalogService)
    {
        _productsService = productsService;
        PropertyChanged += ProductChanged;
        PropertyChanged += CheckEmpty;
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
            IsRefreshing = true;
            IsBusy = true;
            Categories = (from cat
                          in await _catalogService.GetCategoriesAsync()
                          select cat.Category).ToObservableCollection();
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
    private void ProductChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(CurrentProduct)) return;
        ProductType = CurrentProduct.ProductType;
        ProductCategory = CurrentProduct.Category;
        Model = CurrentProduct.Model;
        Price = CurrentProduct.Price.ToString();
        Info = CurrentProduct.Info;
        ImageLink = CurrentProduct.ImageLink;

        _oldImage ??= CurrentProduct.ImageLink;
    }

    [RelayCommand]
    public async Task ChangeProduct()
    {
        try
        {
            IsBusy = true;
            CurrentProduct.ProductType = ProductType;
            CurrentProduct.Category = ProductCategory;
            CurrentProduct.Model = Model;
            CurrentProduct.Price = Double.Parse(Price);
            CurrentProduct.Info = Info;
            CurrentProduct.ImageLink = ImageLink;

            await _productsService.ChangeProductAsync(CurrentProduct, _oldImage, _image);
            await Shell.Current.DisplayAlert("SUCCESSFUL!", "Product changed", "OK");
            await GoBackAsync();
        }
        catch (Exception)
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not change product!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsBusy = false;
        }
    }
    private void CheckEmpty(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(ProductType)
            && e.PropertyName != nameof(ProductCategory)
            && e.PropertyName != nameof(Model)
            && e.PropertyName != nameof(Info)
            && e.PropertyName != nameof(Price)) return;
        if (string.IsNullOrEmpty(ProductType) ||
            string.IsNullOrEmpty(ProductCategory) ||
            string.IsNullOrEmpty(Model) ||
            string.IsNullOrEmpty(Info) ||
            string.IsNullOrEmpty(Price))
            IsNotEmpty = false;
        else IsNotEmpty = true;
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
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
        await Shell.Current.GoToAsync($"{nameof(AddCategoryPage)}");
    }
}
