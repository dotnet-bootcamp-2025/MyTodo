using TodoApp.Application.DTOs;
using TodoApp.Application;
using TodoApp.Domain;
using TodoApp.Infrastructure;

namespace TodoApp.Tests;

public class TodoTests
{
    private readonly ITodoRepository _repository;
    private readonly TodoService _service;

    public TodoTests()
    {
        _repository = new InMemoryTodoRepository();
        _service = new TodoService(_repository);
    }

    [Fact]
    public async Task CreateTodo_WithValidData_ShouldCreateTodo()
    {
        // Arrange
        var createDto = new CreateTodoDto
        {
            Title = "Test Todo",
            Description = "Test Description"
        };

        // Act
        var result = await _service.CreateTodoAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createDto.Title, result.Title);
        Assert.Equal(createDto.Description, result.Description);
        Assert.False(result.IsCompleted);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task GetAllTodos_ShouldReturnAllTodos()
    {
        // Arrange
        await _service.CreateTodoAsync(new CreateTodoDto { Title = "Todo 1", Description = "Desc 1" });
        await _service.CreateTodoAsync(new CreateTodoDto { Title = "Todo 2", Description = "Desc 2" });

        // Act
        var result = await _service.GetAllTodosAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetTodoById_WithValidId_ShouldReturnTodo()
    {
        // Arrange
        var todo = await _service.CreateTodoAsync(new CreateTodoDto { Title = "Test Todo", Description = "Desc" });

        // Act
        var result = await _service.GetTodoByIdAsync(todo.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(todo.Id, result.Id);
        Assert.Equal(todo.Title, result.Title);
    }

    [Fact]
    public async Task CompleteTodo_ShouldMarkTodoAsCompleted()
    {
        // Arrange
        var todo = await _service.CreateTodoAsync(new CreateTodoDto { Title = "Test Todo", Description = "Desc" });

        // Act
        var completed = await _service.CompleteTodoAsync(todo.Id);
        var result = await _service.GetTodoByIdAsync(todo.Id);

        // Assert
        Assert.True(completed);
        Assert.NotNull(result);
        Assert.True(result.IsCompleted);
        Assert.NotNull(result.CompletedAt);
    }

    [Fact]
    public async Task DeleteTodo_WithValidId_ShouldRemoveTodo()
    {
        // Arrange
        var todo = await _service.CreateTodoAsync(new CreateTodoDto { Title = "Test Todo", Description = "Desc" });

        // Act
        var deleted = await _service.DeleteTodoAsync(todo.Id);
        var todos = await _service.GetAllTodosAsync();

        // Assert
        Assert.True(deleted);
        Assert.Empty(todos);
    }
}