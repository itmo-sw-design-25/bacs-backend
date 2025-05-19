using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaCS.Presentation.MAUI.Views;

using Components;
using ViewModels;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchVm searchVm)
    {
        InitializeComponent();
        BindingContext = searchVm;
    }
}

