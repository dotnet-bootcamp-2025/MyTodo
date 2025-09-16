/*namespace TodoApp.Infrastructure.Repositories;

using System.Collections.Concurrent;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Repositories;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly ConcurrentDictionary<Guid, Todo_old> _todos = new();

    public Task<IEnumerable<Todo_old>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Todo_old>>(_todos.Values.ToList());
    }

    public Task<Todo_old?> GetByIdAsync(Guid id)
    {
        _todos.TryGetValue(id, out var todo);
        return Task.FromResult(todo);
    }

    public Task AddAsync(Todo_old todo)
    {
        _todos.TryAdd(todo.Id, todo);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Todo_old todo)
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