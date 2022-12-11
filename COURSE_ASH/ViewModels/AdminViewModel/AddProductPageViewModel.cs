namespace COURSE_ASH.ViewModels.AdminViewModel;

public partial class AddProductPageViewModel : BaseViewModel
{
    //public static readonly List<string> Categories = new List<string>()

    private FileResult _image;
    private readonly ProductsService _productsService;

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

    public AddProductPageViewModel(ProductsService productsService)
    {
        _productsService = productsService;
        PropertyChanged += CheckAnyFieldEmpty;
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
        if (string.IsNullOrEmpty(ProductType) ||
            string.IsNullOrEmpty(Category) ||
            string.IsNullOrEmpty(Model) ||
            string.IsNullOrEmpty(Info) ||
            string.IsNullOrEmpty(Price) ||
            string.IsNullOrEmpty(ImageLink))
            IsNotEmpty = false;
        else IsNotEmpty = true;
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
            Price = Double.Parse(_price),
            Info = _info,
        };
        await _productsService.AddProductAsync(newProduct, _image);
        await Shell.Current.DisplayAlert("SUCCESSFUL!", "Product added", "OK");
        await Shell.Current.GoToAsync("..");
        IsBusy = false;
    }
}
