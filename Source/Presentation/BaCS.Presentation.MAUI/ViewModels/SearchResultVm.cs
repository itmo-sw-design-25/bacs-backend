namespace BaCS.Presentation.MAUI.ViewModels;

using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services;
using States;

public class SearchResultVm : BaseModelView
{
    private SearchResourcesState state;
    public SearchResultVm(SearchResourcesState state)
    {
        this.state = state;

        ShowMoreButton = new RelayCommand(async () => await SendRequestAsync(), () => state.CanSearchResources);
    }

    public IList<ResourceVm> Resources => state.Responce.Resources;
    public IRelayCommand ShowMoreButton { get; }

    private async Task SendRequestAsync()
    {
        await state.SendRequestAsync();
        OnPropertyChanged(nameof(Resources));
        ShowMoreButton.NotifyCanExecuteChanged();
    }


}
