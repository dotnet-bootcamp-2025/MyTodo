# Phase 3 — Flow Control (Loops & Conditionals), Safer Input, Nicer Menu

**Goal:** Turn the console into a robust menu using loops, `switch`, and guard clauses. Keep functions small and focused.

## What you’ll learn
- Replace if-chains with a clear `switch`
- Use **guard clauses** to avoid deep nesting
- Add safer input helpers for `int` and optional `DateOnly`
- Keep each action in a small function

## Prereqs
- Make sure you have completed Phase 0 (see Phase-2.md)
- change to the `Phase3-LoopsConditionals` branch:
```bash
git checkout Phase3-LoopsConditionals
```

## Steps

### 1) Add a Toggle operation in the store (tiny addition)
We’ll add a `Toggle(int id)` method so we can practice a new command in the menu.
- **File:** `src/TodoApp.Infrastructure/InMemoryTodoStore.cs`
- Add this method (below your existing `Complete`/`Delete`

```csharp
public bool Toggle(int id)
{
    if (!_byId.TryGetValue(id, out var existing)) return false;

    var updated = existing with { IsDone = !existing.IsDone };

    var index = _items.FindIndex(t => t.Id == id);
    if (index >= 0) _items[index] = updated;

    _byId[id] = updated;
    return true;
   }
}
```
- This flips the `IsDone` status of a todo.
> Why here? We’re still in fundamentals. The repository/service split comes later; today we keep things tangible and focused on loops & conditionals.

### 2) Refactor `Program.cs`
- Extract helpers: `ReadRequired`, `ReadInt`, `ReadOptionalDate`, `Confirm`
- Use a single `while(true)` loop that reads a menu choice, routes with `switch`, and calls small action functions: `AddFlow`, `ListFlow`, `CompleteFlow`, `ToggleFlow`, `DeleteFlow`.
- Use guard clauses at the top of each action to handle invalid input early.
- **File:** `src/TodoApp.Console/Program.cs`
- Replace the existing code with this:
```csharp
using TodoApp.Domain;
using TodoApp.Infrastructure;

Console.WriteLine("== MyTodo Console (Phase 3) ==");

// Store + Seed
var store = new InMemoryTodoStore();
store.Seed();

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
        case "q":
        case "Q":
        case "":
            Console.WriteLine("Bye!");
            return;
        default:
            Console.WriteLine("Unknown option. Please choose 1–5 or Q to quit.");
            break;
    }
}

// ---------- Actions (small, focused) ----------

void AddFlow()
{
    var title = ReadRequired("Title");
    if (title is null) return; // guard

    var due = ReadOptionalDate("Due date (yyyy-MM-dd, optional)");
    var created = store.Add(title, due);
    Console.WriteLine($"Created: [{created.Id}] {created.Title}");
}

void ListFlow()
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

void CompleteFlow()
{
    var id = ReadInt("Id to complete");
    if (id is null) return; // guard

    if (store.Complete(id.Value))
        Console.WriteLine("Completed.");
    else
        Console.WriteLine("Not found.");
}

void ToggleFlow()
{
    var id = ReadInt("Id to toggle");
    if (id is null) return; // guard

    if (store.Toggle(id.Value))
        Console.WriteLine("Toggled.");
    else
        Console.WriteLine("Not found.");
}

void DeleteFlow()
{
    var id = ReadInt("Id to delete");
    if (id is null) return; // guard

    if (!Confirm($"Are you sure you want to delete #{id}? (y/N)")) return;

    if (store.Delete(id.Value))
        Console.WriteLine("Deleted.");
    else
        Console.WriteLine("Not found.");
}

// ---------- Helpers (simple and safe) ----------

void PrintMenu()
{
    Console.WriteLine();
    Console.WriteLine("Menu:");
    Console.WriteLine("1) Add");
    Console.WriteLine("2) List");
    Console.WriteLine("3) Complete");
    Console.WriteLine("4) Toggle");
    Console.WriteLine("5) Delete");
    Console.WriteLine("Q) Quit");
    Console.Write("> ");
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
- What changed vs Phase 2?
  - A single loop with a switch routes to small action functions (cleaner than nested logic).
  - Guard clauses at the top of each action stop early on invalid input.
  - Input helpers keep parsing logic out of actions (clear separation) — exactly what the Module 1 flow-control rubric calls for.


### 3) Test the menu
```csharp
cd src/TodoApp.Console
dotnet run
```
- From Visual Studio / Rider, run the `TodoApp.Console` project.
- Start the app → see seeded items
- Exercise: Add, List, Complete, **Toggle**, Delete
- Confirm graceful handling of bad inputs (blank title, bad id, invalid date)

## 4) Commit and push the code:
- Make sure you are in the branch: `Phase3-LoopsConditionals`
- In the Terminal run the following commnads:
   ```bash
   git add .
   git commit -m "Phase3-LoopsConditionals - FirstName LastName"
   git push --set-upstream origin Phase3-LoopsConditionals
   ```

## Acceptance Criteria
- Menu uses `switch` and small functions (no deep nesting).
- All 5 actions work with clear messages and safe input handling.
- Invalid inputs never crash the app.

## Commit message suggestion
`feat: polished menu with safer input, guard clauses, and toggle action`

## PR checklist (student)
- [ ] `Program.cs` uses a `switch` for routing.
- [ ] Input helpers validate and parse safely.
- [ ] Functions are small; early returns used for invalid cases.
- [ ] Toggle implemented and tested.
