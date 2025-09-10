
# Phase 2 — Data Structures & Seeding (In-Memory Store)

**Goal:** Store todos in memory, list them, and seed a few fake items.

## What you’ll learn
- When to use a `List<T>` and a `Dictionary<TKey,TValue>` together
- A simple data store class
- Seeding initial data

## Prereqs
- Make sure you have completed Phase 0 (see Phase-1.md)
- change to the `Phase_2-Data_Structures_Seeding` branch:
```bash
git checkout Phase_2-Data_Structures_Seeding
```

## Steps

### 1) Add the in-memory store
Create `src/TodoApp.Infrastructure/InMemoryTodoStore.cs`:

```csharp
using TodoApp.Domain;

namespace TodoApp.Infrastructure;

public class InMemoryTodoStore
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

    public IEnumerable<Todo> List() => _items;

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

    public bool Delete(int id)
    {
        if (!_byId.Remove(id)) return false;
        var removed = _items.RemoveAll(t => t.Id == id) > 0;
        return removed;
    }

    /// <summary>
    /// Seed some sample tasks for demos.
    /// </summary>
    public void Seed()
    {
        Add("Buy milk", DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
        Add("Finish Module 1 notes", DateOnly.FromDateTime(DateTime.Today.AddDays(2)));
        Add("Call the mechanic");
    }
}
```

## 2) Wire it in the Console app

- Open src/TodoApp.Console/Program.cs and use the store + seed:
```csharp
using TodoApp.Domain;
using TodoApp.Infrastructure;

Console.WriteLine("== MyTodo Console (Phase 2) ==");

var store = new InMemoryTodoStore();
store.Seed();

PrintAll();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("Menu:");
    Console.WriteLine("1) Add");
    Console.WriteLine("2) List");
    Console.WriteLine("3) Complete");
    Console.WriteLine("4) Delete");
    Console.WriteLine("Enter to exit");
    Console.Write("> ");

    var choice = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(choice)) break;

    if (choice == "1")
    {
        Console.Write("Title: ");
        var title = Console.ReadLine() ?? string.Empty;

        Console.Write("Due date (yyyy-MM-dd, optional): ");
        var dueText = Console.ReadLine();

        DateOnly? due = null;
        if (!string.IsNullOrWhiteSpace(dueText) &&
            DateOnly.TryParse(dueText, out var parsed))
        {
            due = parsed;
        }

        var created = store.Add(title, due);
        Console.WriteLine($"Created: [{created.Id}] {created.Title}");
    }
    else if (choice == "2")
    {
        PrintAll();
    }
    else if (choice == "3")
    {
        Console.Write("Id to complete: ");
        if (int.TryParse(Console.ReadLine(), out var id) && store.Complete(id))
            Console.WriteLine("Completed.");
        else
            Console.WriteLine("Not found.");
    }
    else if (choice == "4")
    {
        Console.Write("Id to delete: ");
        if (int.TryParse(Console.ReadLine(), out var id) && store.Delete(id))
            Console.WriteLine("Deleted.");
        else
            Console.WriteLine("Not found.");
    }
    else
    {
        Console.WriteLine("Unknown option.");
    }
}

Console.WriteLine("Bye!");

void PrintAll()
{
    Console.WriteLine();
    Console.WriteLine("Current Todos:");
    foreach (var t in store.All)
    {
        var status = t.IsDone ? "[x]" : "[ ]";
        var due = t.DueDate?.ToString("yyyy-MM-dd") ?? "-";
        Console.WriteLine($"{t.Id,2} {status} {t.Title}  (Due: {due})");
    }
}
```

>Note: This is still a console-first experience. In later phases, we’ll add an ITodoRepository interface and a service to apply Clean Architecture boundaries. For now, keep it simple and tangible.

## 3) Run & Test
- From the terminal:
```bash
cd src/TodoApp.Console
dotnet run
```
- From Visual Studio / Rider: Run the `TodoApp.Console` project as before.
- You should see seeded items immediately.
- Use the menu to Add, List, Complete, or Delete.

## 4) Commit and push the code:
- Make sure you are in the branch: `Phase_2-Data_Structures_Seeding`
- In the Terminal run the following commnads:
   ```bash
   git add .
   git commit -m "Phase_2-Data_Structures_Seeding - FirstName LastName"
   git push --set-upstream origin Phase_2-Data_Structures_Seeding
   ```

## Acceptance Criteria

- A store class holds items in memory and supports: Add, List, Complete, Delete.
- The console app prints seeded items at startup.
- I can add new items and see them in the list.

## Commit message suggestion

`feat: in-memory todo store + seeded data and console menu`

## PR checklist (student)

- InMemoryTodoStore added to Infrastructure.
- Console app references the store and seeds at startup.
- Add/List/Complete/Delete work from the console menu.
- Variable names are clear; methods are small and focused.