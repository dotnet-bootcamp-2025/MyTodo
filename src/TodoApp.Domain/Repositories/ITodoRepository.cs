// # Phase 2 — Data Structures & Seeding (In-Memory Store) (Commented, not in use for the moment)

/*namespace TodoApp.Domain.Repositories;

using TodoApp.Domain.Entities;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetAllAsync();
    Task<Todo?> GetByIdAsync(Guid id);
    Task AddAsync(Todo todo);
    Task UpdateAsync(Todo todo);
    Task DeleteAsync(Guid id);
}*/

// # Phase 5 — Clean Code & SRP (ITodoRepository + TodoService) (Completed)

namespace TodoApp.Domain.Repositories;

public interface ITodoRepository
{
    IReadOnlyList<Todo> All { get; }

    Todo Add(string title, DateOnly? dueDate = null);

    bool TryGet(int id, out Todo todo);

    bool Complete(int id);

    bool Toggle(int id);

    bool Delete(int id);
}