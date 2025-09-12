namespace TodoApp.Domain.Repositories;

using TodoApp.Domain.Entities;

public interface ITodoRepository
{
    Task<IEnumerable<Todo_Old>> GetAllAsync();
    Task<Todo_Old?> GetByIdAsync(Guid id);
    Task AddAsync(Todo_Old todo);
    Task UpdateAsync(Todo_Old todo);
    Task DeleteAsync(Guid id);
}