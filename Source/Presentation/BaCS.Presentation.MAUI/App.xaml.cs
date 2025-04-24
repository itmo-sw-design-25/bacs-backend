namespace BaCS.Presentation.MAUI;

using ViewModels;
using Application = Microsoft.Maui.Controls.Application;

public partial class App : Application
{
    public MainPageVm MainPageVm { get; }

    public App()
    {
        InitializeComponent();
        MainPageVm = new MainPageVm();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new NavigationPage(new MainPage() { BindingContext = MainPageVm }));
    }
}
