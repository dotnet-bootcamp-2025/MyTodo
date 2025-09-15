namespace TodoApp.Domain
{
    using TodoApp.Domain.Entities;

    public interface ITodoRepository
    {
        IReadOnlyList<Todo> All { get; }
        Todo Add(string title, DateOnly? dueDate = null);

        // Cambia el tipo del parámetro 'id' de 'int' a 'Guid'
        bool TryGet(Guid id, out Todo todo);
        bool Complete(Guid id);
        bool Toggle(Guid id);
        bool Delete(Guid id);
    }
}