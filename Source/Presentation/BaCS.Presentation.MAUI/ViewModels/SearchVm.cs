namespace BaCS.Presentation.MAUI.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using Resource = Properties.Resources;

public class SearchVm : ObservableObject
{
    public SearchVm(SearchFilterVm searchFilterVm)
    {
        SearchFilterVm = searchFilterVm;
    }
    public string Title => Resource.Search;

    public SearchFilterVm SearchFilterVm { get; }
}
