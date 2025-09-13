using TodoApp.Domain;

Console.WriteLine("== MyTodo Console ==");
Console.WriteLine("Type a task title and press Enter (or just Enter to exit).");

var id = 1;
while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
        break;
    // Explicit type for clarity
    Todo newTodo = new(id++, input, null, false);

    // 'var' is fine when the type is obvious from the RHS:
    var message = $"Created: [{newTodo.Id}] {newTodo.Title}";
    Console.WriteLine(message);
}
Console.WriteLine("Goodbye!");
