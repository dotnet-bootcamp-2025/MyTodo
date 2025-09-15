using TodoApp.Domain.Entities;
using TodoApp.Domain;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly List<Todo> _items = new();
    // Cambia Dictionary<int, Todo> a Dictionary<Guid, Todo>
    private readonly Dictionary<Guid, Todo> _byId = new();
    // Ya no necesitas _nextId porque Guid se genera automáticamente
    // private int _nextId = 1;

    public IReadOnlyList<Todo> All => _items;

    public Todo Add(string title, DateOnly? dueDate = null)
    {
        // Usa el método de fábrica Todo.Create() que ya genera un Guid
        var todo = Todo.Create(title, description: null, dueDate);
        _items.Add(todo);
        _byId[todo.Id] = todo;
        return todo;
    }

    // Cambia el tipo del parámetro 'id' a Guid
    public bool TryGet(Guid id, out Todo todo) => _byId.TryGetValue(id, out todo);

    // Cambia el tipo del parámetro 'id' a Guid en todos los métodos restantes
    public bool Complete(Guid id)
    {
        if (!_byId.TryGetValue(id, out var existing)) return false;

        // Cambia 'IsDone' a 'IsCompleted'
        var updated = existing with { IsCompleted = true };

        var index = _items.FindIndex(t => t.Id == id);
        if (index >= 0) _items[index] = updated;

        _byId[id] = updated;
        return true;
    }

    public bool Toggle(Guid id)
    {
        if (!_byId.TryGetValue(id, out var existing)) return false;

        // Cambia 'IsDone' a 'IsCompleted'
        var updated = existing with { IsCompleted = !existing.IsCompleted };

        var index = _items.FindIndex(t => t.Id == id);
        if (index >= 0) _items[index] = updated;

        _byId[id] = updated;
        return true;
    }

    public bool Delete(Guid id)
    {
        if (!_byId.Remove(id)) return false;
        var removed = _items.RemoveAll(t => t.Id == id) > 0;
        return removed;
    }
}