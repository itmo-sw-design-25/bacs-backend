namespace BaCS.Presentation.MAUI;

using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;

public class CustomTabBarRenderer : ShellRenderer
{
    public CustomTabBarRenderer(Context context)
        : base(context)
    {
    }

    protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
    {
        return new MyShellToolbarAppearanceTracker(this, shellItem);
    }
}

internal class MyShellToolbarAppearanceTracker : ShellBottomNavViewAppearanceTracker
{
    public MyShellToolbarAppearanceTracker(IShellContext shellContext, ShellItem shellItem) : base(shellContext, shellItem)
    {
    }

    public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
    {
        base.SetAppearance(bottomView, appearance);

        var drawable = new GradientDrawable();
        drawable.SetColor(appearance.EffectiveTabBarBackgroundColor.ToPlatform());
        drawable.SetCornerRadius(50);

        bottomView.SetBackground(drawable);

        if (bottomView.LayoutParameters is ViewGroup.MarginLayoutParams layoutParams)
        {
            layoutParams.SetMargins(20, 20, 20, 20);
            bottomView.LayoutParameters = layoutParams;
        }
    }
}


