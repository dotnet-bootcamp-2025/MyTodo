using System.Linq;
using TodoApp.Domain;
using Todo = TodoApp.Domain.NewTodo; //Added to work with class NewTodo instead of Todo and avoid modify Todo and breaking the code in the rest of the projects
using TodoApp.Domain.Repositories;

namespace TodoApp.Application;

public sealed class NewTodoService
{
    private readonly NewITodoRepository _repo;

    public NewTodoService(NewITodoRepository repo)
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
