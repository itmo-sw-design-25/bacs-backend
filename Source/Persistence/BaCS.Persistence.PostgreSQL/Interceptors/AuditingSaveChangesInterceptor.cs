namespace BaCS.Persistence.PostgreSQL.Interceptors;

using Application.Abstractions.Services;
using Domain.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class AuditingSaveChangesInterceptor(IDateTimeService dateTimeService) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new()
    )
    {
        var now = dateTimeService.UtcNow;
        var dbContext = eventData.Context;

        if (dbContext is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        foreach (var entry in dbContext
                     .ChangeTracker
                     .Entries()
                     .Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            if (entry.Entity is not AuditableEntity auditable) continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    auditable.CreatedAt = now;
                    auditable.UpdatedAt = now;

                    break;
                case EntityState.Modified:
                    auditable.UpdatedAt = now;

                    break;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
