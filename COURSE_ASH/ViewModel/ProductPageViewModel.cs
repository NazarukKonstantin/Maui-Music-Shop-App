namespace COURSE_ASH.ViewModel;

[QueryProperty(nameof(PickedProduct),nameof(PickedProduct))]
public partial class ProductPageViewModel : BaseViewModel
{
    [ObservableProperty]
    Product pickedProduct;

    public ProductPageViewModel()
    {

    }

    [RelayCommand]
    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
