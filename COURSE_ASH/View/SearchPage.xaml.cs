using CommunityToolkit.Mvvm;
namespace COURSE_ASH.View;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext=viewModel;
    }
}