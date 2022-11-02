using CommunityToolkit.Mvvm.ComponentModel;
using COURSE_ASH.Model;
using COURSE_ASH.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace COURSE_ASH.SearchHandlers
{
    public partial class ProductSearchHandler : SearchHandler
    {
        //public readonly BindableProperty ProductsQueryProperty = BindableProperty.Create(nameof(ProductsQuery), typeof(ObservableCollection<Product>),
        //                              typeof(ProductSearchHandler), null);
        //public event PropertyChangedEventHandler ProductsQueryPropertyChanged;
        public ObservableCollection<Product> ProductsQuery { get; set; }
        //{
        //    get
        //    {
        //        return (ObservableCollection<Product>)GetValue(ProductsQueryProperty);
        //    }

        //    set
        //    {
        //        if (value != ProductsQuery)
        //        {
        //            SetValue(ProductsQueryProperty, value);
        //            NotifyPropertyChanged();
        //        }
        //    }
        //}

        //private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        //{
        //    ProductsQueryPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        //public IList<Product> ProductsQuery {
        //  get => (IList<Product>)GetValue(ProductsQueryProperty);
        //set => SetValue(ProductsQueryProperty, value);
        //}
        //public Type SelectedItemNavigationTarget { get; set; }
        public string NavigationRoute { get; set; }

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                ItemsSource = ProductsQuery
                    .Where(product => product.Model.ToLower().Contains(newValue.ToLower()))
                    ;
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);

            // Let the animation complete
            await Task.Delay(1000);

            ShellNavigationState state = (App.Current.MainPage as Shell).CurrentState;
            // The following route works because route names are unique in this application.
            await Shell.Current.GoToAsync($"{nameof(NavigationRoute)}?Model={((Product)item).Model}");
        }
        /*
        public ObservableCollection<Product> Products { get; set; }
        public string NavigationRoute { get; set; }

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);
            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource=null;
            }
            else
            {
                ItemsSource=Products.Where(product => product.Model.ToLowerInvariant()
                .Contains(newValue.ToLowerInvariant())).ToList();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);

            if (string.IsNullOrEmpty(NavigationRoute)) return;

            await Shell.Current.GoToAsync(nameof(NavigationRoute),
                new Dictionary<string, object>
                {
                    [nameof(Product)]=item
                });
        }*/
    }
}
