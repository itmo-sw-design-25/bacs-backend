namespace BaCS.Presentation.MAUI.Views.Components;

using ViewModels;

public partial class ModalBasePage : ContentPage
{
    public ModalBasePage(BaseModelView baseModelView)
    {
        BindingContext = baseModelView;
        NavigationPage.SetHasNavigationBar(this, false);
        InitializeComponent();
    }
}

