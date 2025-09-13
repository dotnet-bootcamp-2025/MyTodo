using System.Linq;
using TodoApp.Domain;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure;
using Todo = TodoApp.Domain.NewTodo; //Added to work with class NewTodo instead of Todo and avoid modify Todo and breaking the code in the rest of the projects

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
    var total = store.All.Count;
    var done = store.All.Count(t => t.IsDone);
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