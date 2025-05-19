using Android.App;
using Android.Content.PM;
using Android.OS;

namespace BaCS.Presentation.MAUI;

using Android.Graphics;
using Android.Views;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density
)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);


    }

    /*void setStatusBarColor(Window window, Color color)
    {
        if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.VanillaIceCream)
        {
            // Android 15+

        }
        else
        {
            // For Android 14 and below
            window.SetStatusBarColor(color);
        }
    }*/
}


