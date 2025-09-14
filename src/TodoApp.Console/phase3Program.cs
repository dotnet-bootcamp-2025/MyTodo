/*
using TodoApp.Domain;
using TodoApp.Infrastructure;

Console.WriteLine("== My Todo App console (Phase 3) ==");

var store = new InMemoryTodoStore();
store.Seed();

while (true)
{
    PrintMenu();

    var choice = Console.ReadLine();

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
            Console.WriteLine("Bye!!!");
            return;

        default:
            Console.WriteLine("Unknown option. Plesae choose 1-5 or Q to quit.");
            break;
    }
}

//------------------ Actions (small, focused) ------------------

void AddFlow()
{
    Console.WriteLine("Title: ");
    var title = Console.ReadLine() ?? string.Empty;

    Console.WriteLine("Due date (yyyy-MM-dd, optional): ");
    var dueText = Console.ReadLine();

    DateOnly? due = null;
    if (!string.IsNullOrWhiteSpace(dueText) && DateOnly.TryParse(dueText, out var parsed))
    {
        due = parsed;
    }

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
        var due = t.DueDate?.ToString("yyyy-MM-dd") == "-";
        Console.WriteLine($"{t.Id,2}) {status} {t.Title} (Due: {due}) ");
    }
}

void CompleteFlow()
{
    var id = ReadInt("Id to complete");
    if (id is null) return; //guard change

    if (store.Complete(id.Value))
        Console.WriteLine("Completed");
    else
        Console.WriteLine("Not found");
}

void ToggleFlow()
{
    var id = ReadInt("Id to complete");

    if (store.Toggle(id.Value))
        Console.WriteLine("Toggled");
    else
        Console.WriteLine("Not found");
}

void DeleteFlow()
{
    var id = ReadInt("Id to Delete");

    if (id is null) return; // guard change

    if (!Confirn($"Are you sure you want to delete #{id}? (y/N)")) return;

    if (store.Delete(id.Value))
        Console.WriteLine("Deleted!");
    else
        Console.WriteLine("Not found!");
}

//------------------ Helpers (simple, safe) ------------------

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
    Console.WriteLine("> ");
}

string? ReadRequired(string label)
{
    Console.WriteLine($"{label}");
    var input = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine($"{label} is required!");
        return null;
    }

    return input;
}

int? ReadInt(string label)
{
    Console.WriteLine($"{label}");
    var raw = Console.ReadLine()?.Trim();
    
    if (!int.TryParse(raw, out var value))
    {
        Console.WriteLine("Please enter a valid integer!");
        return null;
    }

    return value;
}

DateOnly? ReadoptionalDate(string label)
{
    Console.WriteLine($"{label}");
    var raw = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(raw)) return null;

    if (DateOnly.TryParse(raw, out var date))
        return date;

    Console.WriteLine("Invalid date! ignoring.");
    return null;
}

bool Confirn(string prompt)
{
    Console.WriteLine(prompt + " ");
    var ans = Console.ReadLine()?.Trim().ToLowerInvariant();

    return ans is "y" or "yes";
}
*/