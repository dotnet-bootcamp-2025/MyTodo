namespace TodoApp.Domain.Repositories;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetAllAsync();
    Task<Todo?> GetByIdAsync(Guid id);
    Task AddAsync(Todo todo);
    Task UpdateAsync(Todo todo);
    Task DeleteAsync(Guid id);
}