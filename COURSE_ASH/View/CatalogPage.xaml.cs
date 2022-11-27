﻿using COURSE_ASH.ViewModel;
using CommunityToolkit.Maui;

namespace COURSE_ASH.View;

public partial class CatalogPage : ContentPage
{
    public CatalogPage(CatalogViewModel viewModel)
    {
        InitializeComponent();
        BindingContext=viewModel;
    }
}

