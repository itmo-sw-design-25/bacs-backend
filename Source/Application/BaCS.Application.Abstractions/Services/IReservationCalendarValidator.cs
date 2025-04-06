namespace BaCS.Application.Abstractions.Services;

using Domain.Core.Entities;
using Domain.Core.ValueObjects;

public interface IReservationCalendarValidator
{
    void ValidateAndThrow(Reservation reservation, CalendarSettings calendarSettings);
    void ValidateAndThrow(DateTimeInterval interval, CalendarSettings calendarSettings);
}
