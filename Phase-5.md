# Phase 5 — Clean Code & SRP (ITodoRepository + TodoService)

**Goal:** Separate responsibilities:
- **Domain**: contracts & entities (`Todo`, `ITodoRepository`)
- **Application**: use cases (`TodoService`)
- **Infrastructure**: implementation (`InMemoryTodoRepository`)
- **Console**: presentation (menu, input/output), calling the service only

## What you’ll learn
- Interfaces to decouple code
- Small services that enforce simple rules (validation, guard clauses)
- Keeping UI thin (no direct data access anymore)

## Prereqs
- Make sure you have completed Phase 0 (see Phase-2.md)
- change to the `Phase_5-CleanCode_SRP` branch:
```bash
git checkout Phase_5-CleanCode_SRP
```

## Steps
1) **Add interface** `ITodoRepository` in Domain.
	- **File:** src/TodoApp.Domain/ITodoRepository.cs (new)

```csharp
namespace TodoApp.Domain;

public interface ITodoRepository
{
    IReadOnlyList<Todo> All { get; }

    Todo Add(string title, DateOnly? dueDate = null);

    bool TryGet(int id, out Todo todo);

    bool Complete(int id);

    bool Toggle(int id);

    bool Delete(int id);
}
```

2) **Refactor Infra**: implement `InMemoryTodoRepository`.
    - **File:** src/TodoApp.Infrastructure/InMemoryTodoRepository.cs (new, refactor from your previous store)
```csharp
namespace TodoApp.Domain;

using TodoApp.Domain;

namespace TodoApp.Infrastructure;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly List<Todo> _items = new();
    private readonly Dictionary<int, Todo> _byId = new();
    private int _nextId = 1;

    public IReadOnlyList<Todo> All => _items;

    public Todo Add(string title, DateOnly? dueDate = null)
    {
        var todo = new Todo(_nextId++, title, dueDate, false);
        _items.Add(todo);
        _byId[todo.Id] = todo;
        return todo;
    }

    public bool TryGet(int id, out Todo todo) => _byId.TryGetValue(id, out todo);

    public bool Complete(int id)
    {
        if (!_byId.TryGetValue(id, out var existing)) return false;
        var updated = existing with { IsDone = true };

        var index = _items.FindIndex(t => t.Id == id);
        if (index >= 0) _items[index] = updated;

        _byId[id] = updated;
        return true;
    }

    public bool Toggle(int id)
    {
        if (!_byId.TryGetValue(id, out var existing)) return false;
        var updated = existing with { IsDone = !existing.IsDone };

        var index = _items.FindIndex(t => t.Id == id);
        if (index >= 0) _items[index] = updated;

        _byId[id] = updated;
        return true;
    }

    public bool Delete(int id)
    {
        if (!_byId.Remove(id)) return false;
        var removed = _items.RemoveAll(t => t.Id == id) > 0;
        return removed;
    }
}

```

3) **Add Application service** `TodoService` with methods:
   - Create, Complete, Toggle, Delete, ListAll, Search, PendingSorted, Stats, NextUp
   - **File:** src/TodoApp.Application/TodoService.cs (new)

```csharp
using System.Linq;
using TodoApp.Domain;

namespace TodoApp.Application;

public sealed class TodoService
{
    private readonly ITodoRepository _repo;

    public TodoService(ITodoRepository repo)
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

```
4) **Move seeding** to a small infra helper `RepoSeeder` that works against the interface.
    - **File:** src/TodoApp.Infrastructure/RepoSeeder.cs (new)
```csharp
using TodoApp.Domain;

namespace TodoApp.Infrastructure;

public static class RepoSeeder
{
    public static void Seed(ITodoRepository repo)
    {
        repo.Add("Buy milk", DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
        repo.Add("Finish Module 1 notes", DateOnly.FromDateTime(DateTime.Today.AddDays(2)));
        repo.Add("Call the mechanic");
    }
}
```
> We seed through the interface—so the console never needs to know the concrete type.

5) **Update Console**: call `TodoService` instead of the repository.
    - **File:** src/TodoApp.Console/Program.cs (replace with this)
```csharp
using System.Linq;
using TodoApp.Domain;
using TodoApp.Application;
using TodoApp.Infrastructure;

Console.WriteLine("== MyTodo Console (Phase 5) ==");

// Wire up infra + app layers (simple manual wiring for clarity)
ITodoRepository repo = new InMemoryTodoRepository();
RepoSeeder.Seed(repo);
var service = new TodoService(repo);

while (true)
{
    PrintMenu();
    var choice = Console.ReadLine()?.Trim();

    switch (choice)
    {
        case "1":
            AddFlow();
            break;
        case "2":
            ListFlow();
            break;
        case "3":
            CompleteFlow();
            break;
        case "4":
            ToggleFlow();
            break;
        case "5":
            DeleteFlow();
            break;

        case "6":
            SearchFlow();
            break;
        case "7":
            ListPendingFlow();
            break;
        case "8":
            StatsFlow();
            break;
        case "9":
            NextUpFlow();
            break;

        case "q":
        case "Q":
        case "":
            Console.WriteLine("Bye!");
            return;
        default:
            Console.WriteLine("Unknown option. Choose 1–9 or Q to quit.");
            break;
    }
}

// ---------- Actions (call service; keep UI thin) ----------

void AddFlow()
{
    var title = ReadRequired("Title");
    if (title is null) return;

    var due = ReadOptionalDate("Due date (yyyy-MM-dd, optional)");
    var result = service.Create(title, due);

    if (!result.Ok)
    {
        Console.WriteLine(result.Error);
        return;
    }

    Console.WriteLine($"Created: [{result.Created!.Id}] {result.Created.Title}");
}

void ListFlow()
{
    Console.WriteLine();
    Console.WriteLine("All Todos:");
    PrintTodos(service.ListAll());
}

void CompleteFlow()
{
    var id = ReadInt("Id to complete");
    if (id is null) return;

    var result = service.Complete(id.Value);
    Console.WriteLine(result.Ok ? "Completed." : result.Error);
}

void ToggleFlow()
{
    var id = ReadInt("Id to toggle");
    if (id is null) return;

    var result = service.Toggle(id.Value);
    Console.WriteLine(result.Ok ? "Toggled." : result.Error);
}

void DeleteFlow()
{
    var id = ReadInt("Id to delete");
    if (id is null) return;
    if (!Confirm($"Are you sure you want to delete #{id}? (y/N)")) return;

    var result = service.Delete(id.Value);
    Console.WriteLine(result.Ok ? "Deleted." : result.Error);
}

// ---------- LINQ flows (unchanged behavior; now via service) ----------

void SearchFlow()
{
    var term = ReadRequired("Search term");
    if (term is null) return;

    Console.WriteLine();
    Console.WriteLine($"Search results for \"{term}\":");
    PrintTodos(service.Search(term));
}

void ListPendingFlow()
{
    Console.WriteLine();
    Console.WriteLine("Pending (sorted by due date):");
    PrintTodos(service.PendingSorted());
}

void StatsFlow()
{
    var (total, done, pending, hasOverdue) = service.Stats();

    Console.WriteLine();
    Console.WriteLine("Stats:");
    Console.WriteLine($"- Total:   {total}");
    Console.WriteLine($"- Done:    {done}");
    Console.WriteLine($"- Pending: {pending}");
    Console.WriteLine($"- Overdue pending exists: {(hasOverdue ? "Yes" : "No")}");
}

void NextUpFlow()
{
    var nextUp = service.NextUp();

    Console.WriteLine();
    if (nextUp is null)
    {
        Console.WriteLine("Next up: (none)");
    }
    else
    {
        Console.WriteLine("Next up:");
        PrintTodos(new[] { nextUp });
    }
}

// ---------- Helpers (same as before) ----------

void PrintMenu()
{
    Console.WriteLine();
    Console.WriteLine("Menu:");
    Console.WriteLine("1) Add");
    Console.WriteLine("2) List");
    Console.WriteLine("3) Complete");
    Console.WriteLine("4) Toggle");
    Console.WriteLine("5) Delete");
    Console.WriteLine("6) Search");
    Console.WriteLine("7) List Pending (sorted)");
    Console.WriteLine("8) Stats");
    Console.WriteLine("9) Next Up");
    Console.WriteLine("Q) Quit");
    Console.Write("> ");
}

void PrintTodos(IEnumerable<Todo> items)
{
    var any = false;
    foreach (var t in items)
    {
        any = true;
        var status = t.IsDone ? "[x]" : "[ ]";
        var due = t.DueDate?.ToString("yyyy-MM-dd") ?? "-";
        Console.WriteLine($"{t.Id,2} {status} {t.Title}  (Due: {due})");
    }
    if (!any)
    {
        Console.WriteLine("(no items)");
    }
}

string? ReadRequired(string label)
{
    Console.Write($"{label}: ");
    var input = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine($"{label} is required.");
        return null;
    }
    return input;
}

int? ReadInt(string label)
{
    Console.Write($"{label}: ");
    var raw = Console.ReadLine()?.Trim();
    if (!int.TryParse(raw, out var value))
    {
        Console.WriteLine("Please enter a valid integer.");
        return null;
    }
    return value;
}

DateOnly? ReadOptionalDate(string label)
{
    Console.Write($"{label}: ");
    var raw = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(raw)) return null;

    if (DateOnly.TryParse(raw, out var date))
        return date;

    Console.WriteLine("Invalid date. Ignoring.");
    return null;
}

bool Confirm(string prompt)
{
    Console.Write(prompt + " ");
    var ans = Console.ReadLine()?.Trim().ToLowerInvariant();
    return ans is "y" or "yes";
}
```

6) Run & verify
- Run the console: `dotnet run` in `src/TodoApp.Console`
- Verify all menu actions work as before.

7) Commit & push
- Make sure you are in the branch: `Phase_5-CleanCode_SRP`
- In the Terminal run the following commnads:
   ```bash
   git add .
   git commit -m "Phase_5-CleanCode_SRP - FirstName LastName"
   git push --set-upstream origin Phase_5-CleanCode_SRP
   ```

## Acceptance Criteria
- Console compiles and runs with **no direct dependency** on a concrete store.
- All menu actions still work exactly as before.
- Code is easier to read: UI → Service → Repository.

## PR checklist (student)
- [ ] `ITodoRepository` added in Domain.
- [ ] `InMemoryTodoRepository` implements the interface (Infra).
- [ ] `TodoService` created (Application) with small, focused methods.
- [ ] Console uses `TodoService` only; no direct access to Infra.
- [ ] Seeder works via the interface (no concrete types in Console).
