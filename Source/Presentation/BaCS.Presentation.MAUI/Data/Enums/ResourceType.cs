namespace BaCS.Presentation.MAUI.Data.Enums;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter<ResourceType>))]
public enum ResourceType
{
    Unspecified = 0,
    MeetingRoom,
    Workplace
}
