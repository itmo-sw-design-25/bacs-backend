namespace BaCS.Presentation.MAUI.ViewModels;

using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public abstract class BaseModelView : ObservableObject
{
    public BaseModelView()
    {
        GoBack = new RelayCommand(() => Shell.Current.Navigation.PopAsync(true));
    }

    public ICommand GoBack { get; }
}
