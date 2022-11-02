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
        public ObservableCollection<Product> Products { get; set; }
        public static List<Product> ProductList { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsTabNotVisible))]
        private bool _isTabVisible;
        public bool IsTabNotVisible => !IsTabVisible;
        private string _filterType;

        public ProductsViewModel()
        {
            Products;

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
