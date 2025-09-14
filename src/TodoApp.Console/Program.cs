using TodoApp.Domain;
using TodoApp.Infrastructure;

Console.WriteLine("== My Todo App console (Phase 2) ==");

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
    Console.WriteLine("> ");

    var choice = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(choice)) break;

    if (choice == "1")
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
    else if (choice == "2")
    {
        PrintAll();
    }
    else if (choice == "3")
    {
        Console.WriteLine("Id to complete: ");
        if (int.TryParse(Console.ReadLine(), out var id) && store.Complete(id))
            Console.WriteLine("Completed");
        else
            Console.WriteLine("Not found");
    }
    else if (choice == "4")
    {
        Console.WriteLine("Id to delete: ");
        if (int.TryParse(Console.ReadLine(), out var id) && store.Delete(id))
            Console.WriteLine("Deleted!");
        else
            Console.WriteLine("Not found");

    }
    else
    { 
        Console.WriteLine("Unknown option");
    }
}

Console.WriteLine("Bye!!!");

void PrintAll()
{
    Console.WriteLine();
    Console.WriteLine("Current Todos:");

    foreach(var t in store.All)
    {
        var status = t.IsDone ? "[x]" : "[ ]";
        var due = t.DueDate?.ToString("yyyy-MM-dd") == "-";
        Console.WriteLine($"{t.Id,2}) {status} {t.Title} (Due: {due}) ");
    }

}