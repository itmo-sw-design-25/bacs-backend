namespace BaCS.Application.Abstractions.Services;

using Domain.Core.Entities;

public interface IReservationCalendarValidator
{
    void ValidateAndThrow(Reservation reservation, CalendarSettings calendarSettings);
}
