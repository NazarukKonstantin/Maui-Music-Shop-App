namespace COURSE_ASH.ViewModels.AdminViewModels;

[QueryProperty (nameof(Category),nameof(Category))]
public partial class EditCategoryPageViewModel : BaseViewModel
{
    private FileResult _image;
    private readonly CatalogService _catalogService;
    private readonly ProductsService _productsService;

    [ObservableProperty]
    private CatalogItem _category;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _imageLink;

    [ObservableProperty]
    private double _imageRotation;

    [ObservableProperty]
    private double _imageScale;

    [ObservableProperty]
    bool isNotEmpty = false;

    public EditCategoryPageViewModel(CatalogService catalogService, ProductsService productsService)
    {
        _catalogService = catalogService;
        _productsService = productsService;
        PropertyChanged += CategoryChanged;
        PropertyChanged += CheckEmpty;
    }

    void CategoryChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(Category)) return;
        Name=Category.Category;
        ImageLink = Category.ImageLink;
        ImageRotation = Category.ImageRotation;
        ImageScale = Category.ImageScale;
    }

    [RelayCommand]
    public async Task ChangeCategory()
    {
        try
        {
            IsBusy = true;
            string oldName = Category.Category;
            Category.Category = Name;
            Category.ImageScale = ImageScale;
            Category.ImageRotation = ImageRotation;
            Category.ImageLink = ImageLink;

            await _catalogService.ChangeCategoryAsync(oldName, Category, _productsService, _image);
            await Shell.Current.DisplayAlert("SUCCESSFUL!", "Product changed", "OK");
            await GoBackAsync();
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("ERROR", "Could not change category!", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void CheckEmpty(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IsNotEmpty)) return;
        IsNotEmpty = !(string.IsNullOrEmpty(Name) ||
                       string.IsNullOrEmpty(ImageLink));
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        Name=string.Empty;
        ImageLink=string.Empty;
        ImageRotation=0;
        ImageScale=0;
        _image=default;
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task PickImage()
    {
        _image = await MediaPicker.PickPhotoAsync();
        if (_image is null) return;
        ImageLink = _image.FullPath;
    }
}
