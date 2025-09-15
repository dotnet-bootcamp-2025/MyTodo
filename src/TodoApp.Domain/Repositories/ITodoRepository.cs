namespace TodoApp.Domain.Repositories;

using TodoApp.Domain.Entities;

public interface ITodoRepository
{
    Task<IEnumerable<Todo_old>> GetAllAsync();
    Task<Todo_old?> GetByIdAsync(Guid id);
    Task AddAsync(Todo_old todo);
    Task UpdateAsync(Todo_old todo);
    Task DeleteAsync(Guid id);
}