using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using COURSE_ASH.Model;

namespace COURSE_ASH.ViewModel
{
    [QueryProperty(nameof(Product), nameof(Product))]
    public partial class ProductDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        Product product;

        public ProductDetailsViewModel()
        {

        }

        [RelayCommand]
        public async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
