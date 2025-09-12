using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Infrastructure;


Console.WriteLine("== MyTodo Console ==");
Console.WriteLine("Type a task title and press Enter (or just Enter to exit).");

var id = 1;
while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
        break;

    // explicit type for clarity
    Todo newTodo = new(id++, input, null, false);

    // 'var' is fine when the type is obvious from the RHS:
    var message = $"Created: [{newTodo.Id}] {newTodo.Title}";
    Console.WriteLine(message);
}

Console.WriteLine("Bye!");
