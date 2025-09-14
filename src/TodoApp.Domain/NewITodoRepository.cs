using Todo = TodoApp.Domain.NewTodo; //Added to work with class NewTodo instead of Todo and avoid modify Todo and breaking the code in the rest of the projects

namespace TodoApp.Domain;

public interface NewITodoRepository
{
    IReadOnlyList<Todo> All { get; }

    Todo Add(string title, DateOnly? dueDate = null);

    bool TryGet(int id, out Todo todo);

    bool Complete(int id);

    bool Toggle(int id);

    bool Delete(int id);
}