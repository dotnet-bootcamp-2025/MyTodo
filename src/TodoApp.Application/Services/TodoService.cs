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
        var todo = Todo_Old.Create(createTodoDto.Title, createTodoDto.Description);
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

    private static TodoDto MapToDto(Todo_Old todo)
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
}