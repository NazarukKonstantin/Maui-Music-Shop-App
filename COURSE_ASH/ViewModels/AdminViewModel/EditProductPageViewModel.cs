namespace COURSE_ASH.ViewModels.AdminViewModel;

[QueryProperty(nameof(Product),nameof(Product))]
public partial class EditProductPageViewModel : BaseViewModel
{
    //public static readonly List<string> Categories;

    private FileResult _image = null;
    private readonly ProductsService _productsService;

    [ObservableProperty]
    private Product _currentProduct;

    [ObservableProperty]
    private string _productType;

    [ObservableProperty]
    private string _category;

    [ObservableProperty]
    private string _model;

    [ObservableProperty]
    private double _price;

    [ObservableProperty]
    private string _info;

    [ObservableProperty]
    private string _imageLink;

    public EditProductPageViewModel(ProductsService productsService)
    {
        _productsService = productsService;
        PropertyChanged += ProductChanged;
    }


    void ProductChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(CurrentProduct)) return;
        ProductType = CurrentProduct.ProductType;
        Category = CurrentProduct.Category;
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
        CurrentProduct.Category= Category;
        CurrentProduct.Model= Model;
        CurrentProduct.Price = Price;
        CurrentProduct.Info= Info;
        CurrentProduct.ImageLink = ImageLink;

        await _productsService.ChangeProductAsync(CurrentProduct, _image);
        await Shell.Current.DisplayAlert("SUCCESSFUL!", "Product changed", "OK");
        await Shell.Current.GoToAsync("..");
        IsBusy = false;
    }

    [RelayCommand]
    public async Task PickImage()
    {
        _image = await MediaPicker.PickPhotoAsync();
        if (_image is null) return;
        ImageLink = _image.FullPath;
    }
}
