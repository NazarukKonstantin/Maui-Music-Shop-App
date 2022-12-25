namespace COURSE_ASH.Views;

public partial class SearchPage : ContentPage
{
    public IRefreshable ViewModel { get; set; }

    public SearchPage(SearchPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        BindingContext=viewModel;
        
    }

    //private void FilterButtonPressed(object sender, EventArgs e)
    //{
    //    _viewModel.IsFilterViewVisible=true;
    //}

    //private void OutOfPopUpTapped(object sender, EventArgs e)
    //{
    //    _viewModel.IsFilterViewVisible=false;
    //}
}