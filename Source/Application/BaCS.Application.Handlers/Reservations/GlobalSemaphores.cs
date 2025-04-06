namespace BaCS.Application.Handlers.Reservations;

using System.Collections.Concurrent;

public static class GlobalSemaphores
{
    private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> Locks = new();

    public static SemaphoreSlim GetForResource(Guid resourceId) =>
        Locks.GetOrAdd(resourceId, _ => new SemaphoreSlim(1, 1));
}
