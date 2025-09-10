# Phase 4 — LINQ Basics (Search, Sort, Stats, Next Up)

**Goal:** Use LINQ to query your in-memory todos with clear, readable code.

## What you’ll learn
- Core LINQ operators: `Where`, `OrderBy`, `Select`, `Count`, `Any`, `FirstOrDefault`
- When to use LINQ instead of manual loops
- Keep methods small; no side effects inside LINQ chains

## New Commands
- **Search**: title contains a term (case-insensitive)
- **List Pending (sorted)**: pending tasks ordered by due date
- **Stats**: quick counts and a simple overdue flag
- **Next Up**: the next pending task by earliest due date

## Prereqs
- Make sure you have completed Phase 0 (see Phase-2.md)
- change to the `Phase_4-LINQ_Fundamentals` branch:
```bash
git checkout Phase_4-LINQ_Fundamentals
```

## Steps

1) **Extend the Console menu** with 4 new commands.
- **File:** `src/TodoApp.Console/Program.cs`
- Replace the file with this version (it keeps all Phase 3 features and adds the LINQ flows):
```csharp
using System.Linq;
using TodoApp.Domain;
using TodoApp.Infrastructure;

Console.WriteLine("== MyTodo Console (Phase 4) ==");

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

        // NEW (LINQ basics)
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

// ---------- Actions (small, focused) ----------

void AddFlow()
{
    var title = ReadRequired("Title");
    if (title is null) return;

    var due = ReadOptionalDate("Due date (yyyy-MM-dd, optional)");
    var created = store.Add(title, due);
    Console.WriteLine($"Created: [{created.Id}] {created.Title}");
}

void ListFlow()
{
    Console.WriteLine();
    Console.WriteLine("All Todos:");
    PrintTodos(store.All);
}

void CompleteFlow()
{
    var id = ReadInt("Id to complete");
    if (id is null) return;

    if (store.Complete(id.Value))
        Console.WriteLine("Completed.");
    else
        Console.WriteLine("Not found.");
}

void ToggleFlow()
{
    var id = ReadInt("Id to toggle");
    if (id is null) return;

    if (store.Toggle(id.Value))
        Console.WriteLine("Toggled.");
    else
        Console.WriteLine("Not found.");
}

void DeleteFlow()
{
    var id = ReadInt("Id to delete");
    if (id is null) return;

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
    Console.WriteLine("6) Search");                 // NEW
    Console.WriteLine("7) List Pending (sorted)");  // NEW
    Console.WriteLine("8) Stats");                  // NEW
    Console.WriteLine("9) Next Up");                // NEW
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
2) **Add flows**: `SearchFlow`, `ListPendingFlow`, `StatsFlow`, `NextUpFlow`.
  - `SearchFlow`: prompt for a term, use `Where` + `Contains` (case-insensitive) to filter, then print.
  - `ListPendingFlow`: use `Where` to filter `IsDone == false`, then `OrderBy` due date (nulls last), then print.
  - `StatsFlow`: use `Count` and `Any` to get total, done, pending, and overdue (due date < today and not done), then print.
  - NextUpFlow`: use `Where` to filter pending, then `OrderBy` due date, then `FirstOrDefault` to get the next item, then print or say "none".
  ```csharp
  // ---------- NEW: LINQ flows ----------

void SearchFlow()
{
    var term = ReadRequired("Search term");
    if (term is null) return;

    var results = store.All
        .Where(t => t.Title.Contains(term, StringComparison.OrdinalIgnoreCase));

    Console.WriteLine();
    Console.WriteLine($"Search results for \"{term}\":");
    PrintTodos(results);
}

void ListPendingFlow()
{
    var pending = store.All
        .Where(t => !t.IsDone)
        .OrderBy(t => t.DueDate ?? DateOnly.MaxValue);

    Console.WriteLine();
    Console.WriteLine("Pending (sorted by due date):");
    PrintTodos(pending);
}

void StatsFlow()
{
    var total   = store.All.Count;
    var done    = store.All.Count(t => t.IsDone);
    var pending = total - done;

    var today = DateOnly.FromDateTime(DateTime.Today);
    var hasOverdue = store.All.Any(t =>
        t.DueDate is { } d && d < today && !t.IsDone);

    Console.WriteLine();
    Console.WriteLine("Stats:");
    Console.WriteLine($"- Total:   {total}");
    Console.WriteLine($"- Done:    {done}");
    Console.WriteLine($"- Pending: {pending}");
    Console.WriteLine($"- Overdue pending exists: {(hasOverdue ? "Yes" : "No")}");
}

void NextUpFlow()
{
    var nextUp = store.All
        .Where(t => !t.IsDone)
        .OrderBy(t => t.DueDate ?? DateOnly.MaxValue)
        .FirstOrDefault();

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
```
3) **Refactor printing** to a helper: `PrintTodos(IEnumerable<Todo> items)`.
    - It takes any enumerable of todos and prints them.
```csharp
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
```

## 4) Run & Test
```bash
cd src/TodoApp.Console
dotnet run
```
- From Visual Studio / Rider, run the `TodoApp.Console` project.

## 5) Commit and push the code:
- Make sure you are in the branch: `Phase_4-LINQ_Fundamentals`
- In the Terminal run the following commnads:
   ```bash
   git add .
   git commit -m "Phase_4-LINQ_Fundamentals - FirstName LastName"
   git push --set-upstream origin Phase_4-LINQ_Fundamentals
   ```


## Acceptance Criteria

New menu entries work: Search, List Pending (sorted), Stats, Next Up.

Simple LINQ replaces manual loops where it improves clarity.

No side effects inside LINQ (just query, then print).

## Commit message suggestion

`feat: LINQ basics — search, pending-sorted, stats, next-up`

## PR checklist (student)

- [ ] Menu shows the 4 new commands.
- [ ] Search, Pending (sorted), Stats, and Next Up behave as described.
- [ ] I used small functions and clear names.
- [ ] No unnecessary temporary lists; I used ToList() only when needed for printing.