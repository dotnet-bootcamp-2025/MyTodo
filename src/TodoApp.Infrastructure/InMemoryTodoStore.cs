using TodoApp.Domain;

namespace TodoApp.Infrastructure;

public class InMemoryTodoStore
{
    //  read that the read only option doesn't let initialize again the same data structure
    // I was thinking that doesn't let to write or modify the data structure
    public readonly List<Todo> _items = new();
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

    public IEnumerable<Todo> List() => _items;

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

    public bool Delete(int id)
    {
        if (!_byid.Remove(id)) return false;

        var removed = _items.RemoveAll(t => t.Id == id) > 0;

        return removed;
    }

    /// <sumary>
    /// Seed some sample data to demo
    /// </sumary>
    ///
    public void Seed()
    {
        Add("Buy cream", DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
        Add("Finish module 1 exercises", DateOnly.FromDateTime(DateTime.Today.AddDays(2)));
        Add("Feed the cat");
    }

}