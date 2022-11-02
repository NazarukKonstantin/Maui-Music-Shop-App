using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using COURSE_ASH.Model;
using COURSE_ASH.View;
using System.Collections.ObjectModel;

namespace COURSE_ASH.ViewModel
{
    [QueryProperty(nameof(_filterType), "FilterType")]
    [QueryProperty(nameof(IsTabVisible), nameof(IsTabVisible))]
    public partial class ProductsViewModel : BaseViewModel
    {
        private readonly Product[] _productList =
        {
            new("Guitar","lol_1","Good guitar",132.42M,"guitar_catalog.png",3),
            new("MIDI", "Model_1", "Good midi", 164.3423M, "midi_catalog.png",5),
            new("Sax", "lol_2", "Good sax", 562.42M, "sax_catalog.png",2),
            new("Ukulele", "Model_1", "Good ukulele", 32.42M, "ukulele_catalog.png",1),
            new("Violin", "lol_3", "Good violin", 322.42M, "violin_catalog.png",4),
            new("Guitar Accessory", "Model_1", "Good shit", 232.42M, "guitar_accessories_catalog.png",0)
        };
        public ObservableCollection<Product> Products { get; set; }
        public static List<Product> ProductList { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsTabNotVisible))]
        private bool _isTabVisible;
        public bool IsTabNotVisible => !IsTabVisible;
        private string _filterType;

        public ProductsViewModel()
        {
            Products = new ObservableCollection<Product>(_productList);

            ProductList=Products.ToList();
            IsTabVisible=true;
        }

        [RelayCommand]
        async Task GoToDetailsAsync(Product product)
        {
            if (product is null)
                return;
            await Shell.Current.GoToAsync($"{nameof(ProductPage)}", true,
                new Dictionary<string, object>
                {
                    [nameof(Product)]=product
                });
        }

        [RelayCommand]
        public async Task GoBackAsync()
        {
            IsTabVisible=true;
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public void FilterProducts(string Text)
        {
            if (string.IsNullOrWhiteSpace(Text))
                Text=string.Empty;
            if (string.IsNullOrWhiteSpace(_filterType))
                _filterType=Text;

            var filteredItems = Products
            .Where(value => value.Model
            .ToLowerInvariant()
            .Contains(_filterType
            .ToLowerInvariant()))
            .ToList();

            if (string.IsNullOrWhiteSpace(Text))
                filteredItems=_productList.ToList();

            foreach (var item in _productList)
            {
                if (!filteredItems.Contains(item))
                {
                    Products.Remove(item);
                }
                else if (!Products.Contains(item))
                {
                    Products.Add(item);
                }
            }
            _filterType=string.Empty;
        }

    }
}
