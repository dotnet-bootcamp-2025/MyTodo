using TodoApp.Domain;

Console.WriteLine("== My Todo App console ==");
Console.WriteLine("Type a task title and press Enter (or just Enter to exit)");

var id = 1;

while (true)
{
    Console.WriteLine("> ");

    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
        break;

    // explicit type of clarity (classic)
    Todo newTodo = new(id++, input, null, false);

    // 'var' is fine when the type is obvious
    var message = $"Created: [{ newTodo.Id }] { newTodo.Title }";

    Console.Write(message);

}

Console.WriteLine("Bye!!!");

