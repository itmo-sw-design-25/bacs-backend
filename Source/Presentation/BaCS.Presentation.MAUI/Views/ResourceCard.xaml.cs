using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaCS.Presentation.MAUI.Views;

public partial class ResourceCard : ContentView
{
    public ResourceCard()
    {
        InitializeComponent();
    }

    public void OnBackButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}

