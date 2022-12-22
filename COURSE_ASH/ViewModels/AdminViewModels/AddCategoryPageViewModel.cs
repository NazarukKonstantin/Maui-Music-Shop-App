namespace COURSE_ASH.ViewModels.AdminViewModels;

public partial class AddCategoryPageViewModel : BaseViewModel
{
    private FileResult _image;
    private readonly CatalogService _service;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _imageLink;

    [ObservableProperty]
    private double _imageRotation;

    [ObservableProperty]
    private double _imageScale;

    [ObservableProperty]
    private bool isNotEmpty = false;

    public AddCategoryPageViewModel(CatalogService service)
    {
        _service = service;
        PropertyChanged += CheckEmpty;
    }
    private void CheckEmpty(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IsNotEmpty)) return;
        IsNotEmpty = !(string.IsNullOrEmpty(Name) ||
                       string.IsNullOrEmpty(ImageLink));
    }

    [RelayCommand]
    private async Task ConfirmAsync()
    {
        try
        {
            bool isSuccessful = await _service.AddCategory(Name, _image, ImageRotation, ImageScale);
            if (!isSuccessful)
            {
                await Shell.Current.DisplayAlert("ERROR!", "Category already exists", "OK");
                IsBusy = false;
                return;
            }
            await Shell.Current.DisplayAlert("SUCCESS!", "Category created", "OK");
            await GoBackAsync();
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("ERROR", "Could not create category!", "OK");
        }
        finally
        {
            IsBusy = false;
        }
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
