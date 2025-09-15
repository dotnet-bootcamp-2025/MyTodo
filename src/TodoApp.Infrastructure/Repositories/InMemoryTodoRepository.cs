// # Phase 2 — Data Structures & Seeding (In-Memory Store) (Commented, not in use for the moment)

/*namespace TodoApp.Infrastructure.Repositories;

using System.Collections.Concurrent;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Repositories;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly ConcurrentDictionary<Guid, Todo> _todos = new();

    public Task<IEnumerable<Todo>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Todo>>(_todos.Values.ToList());
    }

    public Task<Todo?> GetByIdAsync(Guid id)
    {
        _todos.TryGetValue(id, out var todo);
        return Task.FromResult(todo);
    }

    public Task AddAsync(Todo todo)
    {
        _todos.TryAdd(todo.Id, todo);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Todo todo)
    {
        _todos[todo.Id] = todo;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _todos.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}*/

// # Phase 5 — Clean Code & SRP (ITodoRepository + TodoService) (Completed)
using global::TodoApp.Domain.Repositories;
using TodoApp.Domain;

namespace TodoApp.Infrastructure.Repositories;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly List<Todo> _items = new();
    private readonly Dictionary<int, Todo> _byId = new();
    private int _nextId = 1;

    public IReadOnlyList<Todo> All => _items;

    public Todo Add(string title, DateOnly? dueDate = null)
    {
        var todo = new Todo(_nextId++, title, dueDate, false);
        _items.Add(todo);
        _byId[todo.Id] = todo;
        return todo;
    }

    public bool TryGet(int id, out Todo todo) => _byId.TryGetValue(id, out todo);

    public bool Complete(int id)
    {
        if (!_byId.TryGetValue(id, out var existing)) return false;
        var updated = existing with { IsDone = true };

        var index = _items.FindIndex(t => t.Id == id);
        if (index >= 0) _items[index] = updated;

        _byId[id] = updated;
        return true;
    }

    public bool Toggle(int id)
    {
        if (!_byId.TryGetValue(id, out var existing)) return false;
        var updated = existing with { IsDone = !existing.IsDone };

        var index = _items.FindIndex(t => t.Id == id);
        if (index >= 0) _items[index] = updated;

        _byId[id] = updated;
        return true;
    }

    public bool Delete(int id)
    {
        if (!_byId.Remove(id)) return false;
        var removed = _items.RemoveAll(t => t.Id == id) > 0;
        return removed;
    }
}