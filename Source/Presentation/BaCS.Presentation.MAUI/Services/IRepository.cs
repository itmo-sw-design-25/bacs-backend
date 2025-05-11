namespace BaCS.Presentation.MAUI.Services;

interface IRepository<T> : IDisposable
    where T : class
{
    IEnumerable<T> GetList(); // получение всех объектов
    T Get(int id); // получение одного объекта по id
    void Create(T item); // создание объекта
    void Update(T item); // обновление объекта
    void Delete(int id); // удаление объекта по id
    void Save();  // сохранение изменений
}
