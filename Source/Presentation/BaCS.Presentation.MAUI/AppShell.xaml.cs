namespace BaCS.Presentation.MAUI;

using ViewModels;
using Views;

#if  ANDROID
using Android.Views;
using ViewModels;
#endif

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        #if ANDROID
        Platform.CurrentActivity.Window.DecorView.SystemUiVisibility =
            (StatusBarVisibility)
            (SystemUiFlags.ImmersiveSticky | SystemUiFlags.HideNavigation |
             SystemUiFlags.Fullscreen | SystemUiFlags.Immersive);
        #endif
    }
}
