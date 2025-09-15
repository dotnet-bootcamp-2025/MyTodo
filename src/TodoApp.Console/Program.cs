// Phase 0 setup complete

/*
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Infrastructure;

var services = new ServiceCollection();
services.AddInfrastructure();
var serviceProvider = services.BuildServiceProvider();

var todoService = serviceProvider.GetRequiredService<ITodoService>();

// Main loop
bool exit = false;
while (!exit)
{
    Console.Clear();
    Console.WriteLine("===== TODO APP =====");
    Console.WriteLine("1. View all todos");
    Console.WriteLine("2. Add new todo");
    Console.WriteLine("3. View todo details");
    Console.WriteLine("4. Update todo");
    Console.WriteLine("5. Delete todo");
    Console.WriteLine("6. Mark todo as completed");
    Console.WriteLine("7. Mark todo as incomplete");
    Console.WriteLine("0. Exit");
    Console.WriteLine();
    Console.Write("Select an option: ");

    if (int.TryParse(Console.ReadLine(), out int option))
    {
        Console.WriteLine();

        switch (option)
        {
            case 1:
                await ViewAllTodos();
                break;
            case 2:
                await AddNewTodo();
                break;
            case 3:
                await ViewTodoDetails();
                break;
            case 4:
                await UpdateTodo();
                break;
            case 5:
                await DeleteTodo();
                break;
            case 6:
                await CompleteTodo();
                break;
            case 7:
                await ResetTodo();
                break;
            case 0:
                exit = true;
                break;
            default:
                Console.WriteLine("Invalid option. Press any key to continue...");
                Console.ReadKey();
                break;
        }
    }
    else
    {
        Console.WriteLine("Invalid input. Press any key to continue...");
        Console.ReadKey();
    }
}

async Task ViewAllTodos()
{
    var todos = await todoService.GetAllTodosAsync();
    if (!todos.Any())
    {
        Console.WriteLine("No todos found.");
    }
    else
    {
        Console.WriteLine("ID\t\t\t\tTitle\t\tStatus");
        Console.WriteLine("---------------------------------------------------------------");
        foreach (var todo in todos)
        {
            Console.WriteLine($"{todo.Id}\t{todo.Title}\t{(todo.IsCompleted ? "Completed" : "Pending")}");
        }
    }
    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}

async Task AddNewTodo()
{
    Console.Write("Enter title: ");
    var title = Console.ReadLine() ?? "";
    
    Console.Write("Enter description: ");
    var description = Console.ReadLine() ?? "";

    var createTodoDto = new CreateTodoDto
    {
        Title = title,
        Description = description
    };

    try
    {
        var newTodo = await todoService.CreateTodoAsync(createTodoDto);
        Console.WriteLine($"Todo created with ID: {newTodo.Id}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}

async Task ViewTodoDetails()
{
    Console.Write("Enter todo ID: ");
    if (Guid.TryParse(Console.ReadLine(), out Guid id))
    {
        var todo = await todoService.GetTodoByIdAsync(id);
        if (todo == null)
        {
            Console.WriteLine("Todo not found.");
        }
        else
        {
            Console.WriteLine($"ID: {todo.Id}");
            Console.WriteLine($"Title: {todo.Title}");
            Console.WriteLine($"Description: {todo.Description}");
            Console.WriteLine($"Status: {(todo.IsCompleted ? "Completed" : "Pending")}");
            Console.WriteLine($"Created: {todo.CreatedAt}");
            if (todo.CompletedAt.HasValue)
            {
                Console.WriteLine($"Completed: {todo.CompletedAt}");
            }
        }
    }
    else
    {
        Console.WriteLine("Invalid ID format.");
    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}

async Task UpdateTodo()
{
    Console.Write("Enter todo ID: ");
    if (Guid.TryParse(Console.ReadLine(), out Guid id))
    {
        var todo = await todoService.GetTodoByIdAsync(id);
        if (todo == null)
        {
            Console.WriteLine("Todo not found.");
        }
        else
        {
            Console.WriteLine($"Current title: {todo.Title}");
            Console.Write("Enter new title (leave empty to keep current): ");
            var title = Console.ReadLine();
            
            Console.WriteLine($"Current description: {todo.Description}");
            Console.Write("Enter new description (leave empty to keep current): ");
            var description = Console.ReadLine();

            var updateTodoDto = new UpdateTodoDto
            {
                Title = string.IsNullOrWhiteSpace(title) ? todo.Title : title,
                Description = string.IsNullOrWhiteSpace(description) ? todo.Description : description
            };

            try
            {
                var updatedTodo = await todoService.UpdateTodoAsync(id, updateTodoDto);
                if (updatedTodo != null)
                {
                    Console.WriteLine("Todo updated successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to update todo.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
    else
    {
        Console.WriteLine("Invalid ID format.");
    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}

async Task DeleteTodo()
{
    Console.Write("Enter todo ID: ");
    if (Guid.TryParse(Console.ReadLine(), out Guid id))
    {
        var result = await todoService.DeleteTodoAsync(id);
        if (result)
        {
            Console.WriteLine("Todo deleted successfully.");
        }
        else
        {
            Console.WriteLine("Todo not found or could not be deleted.");
        }
    }
    else
    {
        Console.WriteLine("Invalid ID format.");
    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}

async Task CompleteTodo()
{
    Console.Write("Enter todo ID: ");
    if (Guid.TryParse(Console.ReadLine(), out Guid id))
    {
        var result = await todoService.CompleteTodoAsync(id);
        if (result)
        {
            Console.WriteLine("Todo marked as completed.");
        }
        else
        {
            Console.WriteLine("Todo not found or could not be updated.");
        }
    }
    else
    {
        Console.WriteLine("Invalid ID format.");
    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}

async Task ResetTodo()
{
    Console.Write("Enter todo ID: ");
    if (Guid.TryParse(Console.ReadLine(), out Guid id))
    {
        var result = await todoService.ResetTodoAsync(id);
        if (result)
        {
            Console.WriteLine("Todo marked as incomplete.");
        }
        else
        {
            Console.WriteLine("Todo not found or could not be updated.");
        }
    }
    else
    {
        Console.WriteLine("Invalid ID format.");
    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}*/
//# Phase 1 — C# Syntax & Variables & Loops (Console I/O + Todo entity) (Completed)
/*
using TodoApp.Domain;

Console.WriteLine("=== MyTodo Console ===");
Console.WriteLine("Type a task title and press Enter (or just Enter to exit):");

var id = 1;
while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
        break;

    // explicit type for clarity
    Todo newTodo = new(id++, input, null, false);

    // 'var' id fine when the type is obvious from the RHS:
    var message = $"Created: [{newTodo.Id}] {newTodo.Title}";
    Console.WriteLine(message);
}

Console.WriteLine("Bye!");
*/

// # Phase 2 — Data Structures & Seeding (In-Memory Store)
/*
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
}*/

// # Phase 3 — Flow Control (Loops & Conditionals), Safer Input, Nicer Menu (Completed)
/*
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
}*/

// # Phase 4 — LINQ Basics (Search, Sort, Stats, Next Up) (Completed)

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