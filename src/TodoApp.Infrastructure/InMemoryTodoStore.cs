using TodoApp.Domain;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure;

public class InMemoryTodoStore
{
    private readonly List<Todo> _items = new();
    private readonly Dictionary<Guid, Todo> _byId = new();

    public IReadOnlyList<Todo> All => _items;

    public Todo Add(string title, string description = "", DateOnly? dueDate = null)
    {
        var todo = Todo.Create(title, description, dueDate);
        _items.Add(todo);
        _byId[todo.Id] = todo;
        return todo;
    }

    public IEnumerable<Todo> List() => _items;

    public bool TryGet(Guid id, out Todo todo) => _byId.TryGetValue(id, out todo);

    public bool Complete(Guid id)
    {
        if (!_byId.TryGetValue(id, out var existing)) return false;

        existing.MarkAsCompleted();
        return true;
    }

    public bool Delete(Guid id)
    {
        if (!_byId.Remove(id)) return false;
        var removed = _items.RemoveAll(t => t.Id == id) > 0;
        return removed;
    }

    /// <summary>
    /// Seed some sample tasks for demos.
    /// </summary>
    public void Seed()
    {
        Add("Buy milk", "", DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
        Add("Finish Module 1 notes", "", DateOnly.FromDateTime(DateTime.Today.AddDays(2)));
        Add("Call the mechanic");
    }
}
