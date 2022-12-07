namespace COURSE_ASH.Views;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext=viewModel;
    }
    
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}