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
    public void Create_WithValidTitle_ShouldCreateTodo()
    {
        // Arrange
        var title = "Test Todo";
        var dueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

        // Act
        var (ok, error, created) = _service.Create(title, dueDate);

        // Assert
        Assert.True(ok);
        Assert.Null(error);
        Assert.NotNull(created);
        Assert.Equal(title, created.Title);
        Assert.Equal(dueDate, created.DueDate);
        Assert.False(created.IsDone);
    }

    [Fact]
    public void Create_WithEmptyTitle_ShouldReturnError()
    {
        // Act
        var (ok, error, created) = _service.Create("", null);

        // Assert
        Assert.False(ok);
        Assert.Equal("Title is required.", error);
        Assert.Null(created);
    }

    [Fact]
    public void ListAll_ShouldReturnAllTodos()
    {
        // Arrange
        _service.Create("Todo 1", null);
        _service.Create("Todo 2", null);

        // Act
        var result = _service.ListAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Complete_WithValidId_ShouldMarkAsCompleted()
    {
        // Arrange
        var (_, _, todo) = _service.Create("Test Todo", null);

        // Act
        var (ok, error) = _service.Complete(todo!.Id);

        // Assert
        Assert.True(ok);
        Assert.Null(error);
        
        var completed = _repository.All.First(t => t.Id == todo.Id);
        Assert.True(completed.IsDone);
    }

    [Fact]
    public void Toggle_ShouldFlipCompletionStatus()
    {
        // Arrange
        var (_, _, todo) = _service.Create("Test Todo", null);

        // Act - Toggle to complete
        var (ok1, _) = _service.Toggle(todo!.Id);
        var afterFirst = _repository.All.First(t => t.Id == todo.Id);

        // Act - Toggle back to incomplete
        var (ok2, _) = _service.Toggle(todo.Id);
        var afterSecond = _repository.All.First(t => t.Id == todo.Id);

        // Assert
        Assert.True(ok1);
        Assert.True(afterFirst.IsDone);
        Assert.True(ok2);
        Assert.False(afterSecond.IsDone);
    }

    [Fact]
    public void Delete_WithValidId_ShouldRemoveTodo()
    {
        // Arrange
        var (_, _, todo) = _service.Create("Test Todo", null);

        // Act
        var (ok, error) = _service.Delete(todo!.Id);

        // Assert
        Assert.True(ok);
        Assert.Null(error);
        Assert.Empty(_service.ListAll());
    }

    [Fact]
    public void Search_ShouldFindMatchingTodos()
    {
        // Arrange
        _service.Create("Buy milk", null);
        _service.Create("Call mechanic", null);
        _service.Create("Buy bread", null);

        // Act
        var results = _service.Search("buy");

        // Assert
        Assert.Equal(2, results.Count());
        Assert.All(results, t => t.Title.Contains("Buy", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Stats_ShouldReturnCorrectCounts()
    {
        // Arrange
        var (_, _, todo1) = _service.Create("Todo 1", null);
        var (_, _, todo2) = _service.Create("Todo 2", null);
        _service.Complete(todo1!.Id);

        // Act
        var (total, done, pending, hasOverdue) = _service.Stats();

        // Assert
        Assert.Equal(2, total);
        Assert.Equal(1, done);
        Assert.Equal(1, pending);
    }
}