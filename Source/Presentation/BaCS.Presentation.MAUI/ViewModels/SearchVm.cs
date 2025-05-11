namespace BaCS.Presentation.MAUI.ViewModels;


using Resource = Properties.Resources;

public class SearchVm : NotifyPropertyChanged
{
    public string Title => Resource.Search;

    public SearchFilterVm SearchFilterVm { get; } = new SearchFilterVm();
}
