using TodoApp.Domain;

namespace TodoApp.Infrastructure;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly List<Todo> _items = new();
    private readonly Dictionary<int, Todo> _byid = new();
    private int _nextId = 1;

    public IReadOnlyList<Todo> All => _items;

    public Todo Add(string title, DateOnly? dueDate = null)
    {
        var todo = new Todo(_nextId++, title, dueDate, false);

        _items.Add(todo);
        _byid[todo.Id] = todo;
        return todo;
    }

    public bool TryGet(int id, out Todo todo) => _byid.TryGetValue(id, out todo);

    public bool Complete(int id)
    {
        if (!_byid.TryGetValue(id, out var existing)) return false;

        var updated = existing with { IsDone = true };

        var index = _items.FindIndex(t => t.Id == id);

        if (index >= 0) _items[index] = updated;

        _byid[id] = updated;

        return true;
    }

    public bool Toggle(int id)
    {
        if (!_byid.TryGetValue(id, out var existing)) return false;

        var updated = existing with { IsDone = !existing.IsDone };

        var index = _items.FindIndex(t => t.Id == id);

        if (index >= 0) _items[index] = updated;

        _byid[id] = updated;

        return true;
    }

    public bool Delete(int id)
    {
        if (!_byid.Remove(id)) return false;

        var removed = _items.RemoveAll(t => t.Id == id) > 0;

        return removed;
    }

}
