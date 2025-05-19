namespace BaCS.Presentation.MAUI.Resources;

using ViewModels;

public partial class SearchResultListItem : ContentView
{
    public SearchResultListItem()
    {
        InitializeComponent();
    }

    public SearchResultListItem(ResourceVm resourceVm) : this()
    {
        BindingContext = resourceVm;
    }
}

