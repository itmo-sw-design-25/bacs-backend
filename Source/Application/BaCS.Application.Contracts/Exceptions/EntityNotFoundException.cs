namespace BaCS.Application.Contracts.Exceptions;

public class EntityNotFoundException<T>(Guid id)
    : NotFoundException($"Сущность {typeof(T).Name} с ID {id} не найдена.")
    where T : class;
