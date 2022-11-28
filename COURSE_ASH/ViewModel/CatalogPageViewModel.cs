using CommunityToolkit.Mvvm.Input;
using COURSE_ASH.Model;
using COURSE_ASH.View;
using System.Collections.ObjectModel;

namespace COURSE_ASH.ViewModel
{
    public partial class CatalogPageViewModel : BaseViewModel
    {
        public ObservableCollection<CatalogItem> Items { get; set; } = new();

        public void HardcodeData()
        {
            Items.Add(new("Guitars", "guitar_catalog", 10, 1.6));
            Items.Add(new("Brass", "sax_catalog", -60, 2));
            Items.Add(new("Accessories", "guitar_accessories_catalog", 0, 1.5));
            Items.Add(new("Strings", "violin_catalog", 18, 1.8));
            Items.Add(new("Ukulele", "ukulele_catalog", 0, 1.9));
            Items.Add(new("Keyboards", "midi_catalog", -10, 1));
        }

        public CatalogPageViewModel()
        {
            HardcodeData();
        }

        [RelayCommand]
        async Task GoToProductsListAsync(CatalogItem item)
        {
            if (item is null) return;
            string FilterType = item.ProductsType.ToLowerInvariant();
            bool IsTabVisible = false;
            await Shell.Current.GoToAsync($"{nameof(SearchPage)}", true,
                new Dictionary<string, object>
                {
                    [nameof(FilterType)]=FilterType,
                    [nameof(IsTabVisible)]=IsTabVisible
                });

        }

        [RelayCommand]
        async Task GoToLoginPage()
        {
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }
    }
}
