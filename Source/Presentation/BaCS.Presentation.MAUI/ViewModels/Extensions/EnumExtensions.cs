namespace BaCS.Presentation.MAUI.Models;

using Properties;
using Services;

public static class EnumExtensions
{
    public static string GetDisplayName(this ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Workplace: return Resources.WorkPlace;
            case ResourceType.MeetingRoom: return Resources.MeetingRoom;
            default: return Resources.Unspecified;
        }
    }
}
