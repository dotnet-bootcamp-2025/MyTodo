//cesar villarreal, great bootcamp!
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