namespace BaCS.Application.Contracts.Exceptions;

public class EntityNotFoundException<T>(Guid id)
    : NotFoundException($"Сущность {typeof(T)} с ID {id} не найдена.")
    where T : class;
