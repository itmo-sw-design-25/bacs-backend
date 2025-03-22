namespace BaCS.Domain.Core.Enums;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter<ResourceType>))]
public enum ResourceType
{
    Unspecified = 0,
    MeetingRoom,
    Workplace
}
