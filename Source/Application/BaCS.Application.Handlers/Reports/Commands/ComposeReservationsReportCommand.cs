namespace BaCS.Application.Handlers.Reports.Commands;

using Abstractions.Persistence;
using Abstractions.Services;
using ClosedXML.Excel;
using Contracts.Exceptions;
using Domain.Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class ComposeReservationsReportCommand
{
    public record Command(
        DateOnly From,
        DateOnly To,
        Guid? UserId,
        Guid? ResourceId,
        Guid? LocationId
    ) : IRequest<Stream>;

    internal class Handler(IBaCSDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<Command, Stream>
    {
        public async Task<Stream> Handle(Command request, CancellationToken cancellationToken)
        {
            if (currentUser.IsSuperAdmin() is false)
                throw new ForbiddenException("Недостаточно прав для получения отчёта по бронированиям.");

            var from = request.From.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            var to = request.To.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);

            var query = dbContext
                .Reservations
                .AsNoTracking()
                .Where(x => x.From >= from && x.To <= to);

            if (request.UserId is { } userId) query = query.Where(x => x.UserId == userId);
            if (request.ResourceId is { } resourceId) query = query.Where(x => x.ResourceId == resourceId);
            if (request.LocationId is { } locationId) query = query.Where(x => x.LocationId == locationId);

            var reservations = await query
                .Select(x => new ReservationProjection
                    {
                        Id = x.Id,
                        User = x.User.Email,
                        Location = x.Location.Name,
                        Resource = x.Resource.Name,
                        From = x.From,
                        To = x.To,
                        Status = x.Status
                    }
                )
                .AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken);

            return ComposeXlsxReport(reservations);
        }

        private static MemoryStream ComposeXlsxReport(List<ReservationProjection> reservations)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Бронирования");
            var row = 1;

            FillRow(ws, row++, ["ID", "Пользователь", "Локация", "Ресурс", "Начало", "Конец", "Статус"]);

            var headerRange = ws.Range(1, 1, 1, 6);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            foreach (var r in reservations)
            {
                FillRow(
                    ws,
                    row++,
                    [
                        r.Id.ToString(),
                        r.User,
                        r.Location,
                        r.Resource,
                        r.From.ToString("u"),
                        r.To.ToString("u"),
                        r.Status.ToString()
                    ]
                );
            }

            ws.Columns().AdjustToContents();
            ws.SheetView.FreezeRows(1);

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return stream;
        }

        private static void FillRow(IXLWorksheet ws, int row, string[] values)
        {
            foreach (var (i, value) in values.Index())
            {
                ws.Cell(row, i + 1).Value = value;
            }
        }
    }

    private sealed record ReservationProjection
    {
        public Guid Id { get; init; }
        public string User { get; init; }
        public string Location { get; init; }
        public string Resource { get; init; }
        public DateTime From { get; init; }
        public DateTime To { get; init; }
        public ReservationStatus Status { get; init; }
    }
}
