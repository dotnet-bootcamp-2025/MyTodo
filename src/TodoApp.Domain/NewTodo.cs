namespace TodoApp.Domain;

public record NewTodo(
    int Id,
    string Title,
    DateOnly? DueDate,
    bool IsDone = false
);