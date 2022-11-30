namespace COURSE_ASH.ViewModel;

public partial class CartPageViewModel : BaseViewModel
{
    public ObservableCollection<Product> CartProducts { get; set; }

    public CartPageViewModel()
    {
        CartProducts=null;
    }
}
