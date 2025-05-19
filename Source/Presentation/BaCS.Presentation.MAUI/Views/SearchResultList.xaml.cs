namespace BaCS.Presentation.MAUI.Views;

using Components;
using ViewModels;

public partial class SearchResultList : ModalBasePage
{
    public SearchResultList(SearchResultVm searchResultVm) : base(searchResultVm)
    {
        InitializeComponent();
    }
}

