namespace COURSE_ASH.Views;

public partial class SearchPage : ContentPage
{
    private SearchPageViewModel _viewModel;

    public SearchPage(SearchPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext=_viewModel;
        
    }
    
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    private void FilterButtonPressed(object sender, EventArgs e)
    {
        _viewModel.IsFilterViewVisible=true;
    }

    private void OutOfPopUpTapped(object sender, EventArgs e)
    {
        _viewModel.IsFilterViewVisible=false;
    }
}