namespace COURSE_ASH.Views.UserViews;

public partial class CatalogPage : ContentPage
{
    private CatalogPageViewModel _viewModel;

    public CatalogPage(CatalogPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel=viewModel;
        BindingContext=viewModel;
    }

    //protected override void OnNavigatedTo(NavigatedToEventArgs args)
    //{
    //    base.OnNavigatedTo(args);
    //    if (string.IsNullOrEmpty(_viewModel.ImageLink))
    //    {
    //        ProfilePic.SetAppTheme(Image.SourceProperty,
    //            Icons.WaltuhBlack, Icons.WaltuhWhite);
    //    }
    //    else
    //    {
    //        ProfilePic.Source.BindingContext=_viewModel.ImageLink;
    //    }
    //}
}

