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