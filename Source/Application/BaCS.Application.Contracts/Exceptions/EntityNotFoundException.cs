namespace BaCS.Application.Contracts.Exceptions;

public class EntityNotFoundException<T> : NotFoundException
    where T : class
{
    public EntityNotFoundException(Guid id)
        : base($"Сущность {typeof(T).Name} с ID {id} не найдена.") { }

    public EntityNotFoundException(string message)
        : base(message) { }
}
