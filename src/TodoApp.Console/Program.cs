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