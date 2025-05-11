using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaCS.Presentation.MAUI.Views;

using System.Globalization;
using Domain.Core.Enums;
using Domain.Core.Extensions;

public partial class LocationFilter : ContentView
{
    public LocationFilter()
    {
        InitializeComponent();
    }
}


#region Nested

public class LocationsToStringConverter : IValueConverter {
    public static LocationsToStringConverter Instance = new LocationsToStringConverter();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Location location)
        {
            return location.ToString();
        }

        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class ResourceTypeToStringConverter : IValueConverter {
    public static ResourceTypeToStringConverter Instance = new ResourceTypeToStringConverter();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ResourceType resourceType)
        {
            return resourceType.GetDisplayName();
        }

        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

#endregion

