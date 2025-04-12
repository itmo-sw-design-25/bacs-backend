namespace BaCS.Domain.Core.Extensions;

using System.ComponentModel.DataAnnotations;
using System.Reflection;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var member = enumValue
            .GetType()
            .GetMember(enumValue.ToString())
            .FirstOrDefault();

        var displayAttribute = member?.GetCustomAttribute<DisplayAttribute>();

        return displayAttribute?.Name ?? enumValue.ToString();
    }
}
