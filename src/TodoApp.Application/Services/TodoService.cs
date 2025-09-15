// # Phase 2 — Data Structures & Seeding (In-Memory Store) (Commented, not in use for the moment)
/*
namespace TodoApp.Application.Services;

using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Repositories;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepository;

    public TodoService(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<IEnumerable<TodoDto>> GetAllTodosAsync()
    {
        var todos = await _todoRepository.GetAllAsync();
        return todos.Select(MapToDto);
    }

    public async Task<TodoDto?> GetTodoByIdAsync(Guid id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        return todo != null ? MapToDto(todo) : null;
    }

    public async Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto)
    {
        var todo = Todo.Create(createTodoDto.Title, createTodoDto.Description);
        await _todoRepository.AddAsync(todo);
        return MapToDto(todo);
    }

    public async Task<TodoDto?> UpdateTodoAsync(Guid id, UpdateTodoDto updateTodoDto)
    {
        var existingTodo = await _todoRepository.GetByIdAsync(id);
        if (existingTodo == null)
            return null;

        existingTodo.UpdateDetails(updateTodoDto.Title, updateTodoDto.Description);
        await _todoRepository.UpdateAsync(existingTodo);
        return MapToDto(existingTodo);
    }

    public async Task<bool> DeleteTodoAsync(Guid id)
    {
        var existingTodo = await _todoRepository.GetByIdAsync(id);
        if (existingTodo == null)
            return false;

        await _todoRepository.DeleteAsync(id);
        return true;
    }

    public async Task<bool> CompleteTodoAsync(Guid id)
    {
        var existingTodo = await _todoRepository.GetByIdAsync(id);
        if (existingTodo == null)
            return false;

        existingTodo.MarkAsCompleted();
        await _todoRepository.UpdateAsync(existingTodo);
        return true;
    }

    public async Task<bool> ResetTodoAsync(Guid id)
    {
        var existingTodo = await _todoRepository.GetByIdAsync(id);
        if (existingTodo == null)
            return false;

        existingTodo.MarkAsIncomplete();
        await _todoRepository.UpdateAsync(existingTodo);
        return true;
    }

    private static TodoDto MapToDto(Todo todo)
    {
        return new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            IsCompleted = todo.IsCompleted,
            CreatedAt = todo.CreatedAt,
            CompletedAt = todo.CompletedAt
        };
    }
}*/

// # Phase 5 — Clean Code & SRP (ITodoRepository + TodoService) (Compl

using System.Linq;
using TodoApp.Domain;
using TodoApp.Domain.Repositories;

namespace TodoApp.Application.Services;

public sealed class TodoService
{
    private readonly ITodoRepository _repo;

    public TodoService(ITodoRepository repo)
    {
        _repo = repo;
    }

    // ---------- Commands ----------

    public (bool Ok, string? Error, Todo? Created) Create(string? title, DateOnly? due)
    {
        if (string.IsNullOrWhiteSpace(title))
            return (false, "Title is required.", null);

        var created = _repo.Add(title.Trim(), due);
        return (true, null, created);
    }

    public (bool Ok, string? Error) Complete(int id)
    {
        return _repo.Complete(id)
            ? (true, null)
            : (false, "Not found.");
    }

    public (bool Ok, string? Error) Toggle(int id)
    {
        return _repo.Toggle(id)
            ? (true, null)
            : (false, "Not found.");
    }

    public (bool Ok, string? Error) Delete(int id)
    {
        return _repo.Delete(id)
            ? (true, null)
            : (false, "Not found.");
    }

    // ---------- Queries (LINQ on repo.All) ----------

    public IReadOnlyList<Todo> ListAll() => _repo.All;

    public IEnumerable<Todo> Search(string term)
    {
        if (string.IsNullOrWhiteSpace(term)) return Enumerable.Empty<Todo>();
        return _repo.All.Where(t => t.Title.Contains(term,
            StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<Todo> PendingSorted()
    {
        return _repo.All
            .Where(t => !t.IsDone)
            .OrderBy(t => t.DueDate ?? DateOnly.MaxValue);
    }

    public (int total, int done, int pending, bool hasOverdue) Stats()
    {
        var total = _repo.All.Count;
        var done = _repo.All.Count(t => t.IsDone);
        var pending = total - done;

        var today = DateOnly.FromDateTime(DateTime.Today);
        var hasOverdue = _repo.All.Any(t =>
            t.DueDate is { } d && d < today && !t.IsDone);

        return (total, done, pending, hasOverdue);
    }

    public Todo? NextUp()
    {
        return _repo.All
            .Where(t => !t.IsDone)
            .OrderBy(t => t.DueDate ?? DateOnly.MaxValue)
            .FirstOrDefault();
    }
}