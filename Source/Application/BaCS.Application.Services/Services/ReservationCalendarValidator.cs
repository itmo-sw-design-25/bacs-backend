namespace BaCS.Application.Services.Services;

using Abstractions.Services;
using Contracts.Exceptions;
using Domain.Core.Constants;
using Domain.Core.Entities;
using Domain.Core.Extensions;
using Domain.Core.ValueObjects;

public class ReservationCalendarValidator : IReservationCalendarValidator
{
    public void ValidateAndThrow(Reservation reservation, CalendarSettings calendarSettings)
    {
        var interval = new DateTimeInterval(reservation.From, reservation.To);

        if (interval.Duration.TotalHours >= DateTimeConstants.HoursPerDay)
        {
            throw new BusinessRulesException(
                $"Время бронирования не может превышать {DateTimeConstants.HoursPerDay} часов"
            );
        }

        if (interval.From.Hour < calendarSettings.AvailableFrom.Hour)
        {
            throw new BusinessRulesException(
                $"Время начала бронирования выбранной локации не может быть раньше {calendarSettings.AvailableFrom:t}"
            );
        }

        if (interval.To.Hour > calendarSettings.AvailableTo.Hour)
        {
            throw new BusinessRulesException(
                $"Время окончания бронирования выбранной локации не может быть позже {calendarSettings.AvailableTo:t}"
            );
        }

        if (interval.From.ToRussianDayOfWeek() != interval.To.ToRussianDayOfWeek())
        {
            throw new BusinessRulesException(
                "Бронирование на выбранные даты должно быть в пределах одного дня недели."
            );
        }

        var dayOfWeek = interval.From.ToRussianDayOfWeek();

        if (calendarSettings.AvailableDaysOfWeek.Contains(dayOfWeek) is false)
        {
            throw new BusinessRulesException(
                $"Бронирование в выбранный день недели {dayOfWeek.ToDisplayName()} недоступно на данной локации."
            );
        }
    }
}
