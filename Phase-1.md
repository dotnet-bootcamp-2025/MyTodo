# Phase 1 — C# Syntax & Variables & Loops (Console I/O + Todo entity)

**Goal:** Practice namespaces, program entry point, classes/records, fields vs properties, `var` vs explicit types, and immutability.

## What you’ll learn
- Create a simple domain entity as a **record**
- Use `Console.ReadLine()` / `Console.WriteLine()` for basic I/O
- Decide when to use `var` vs explicit types

## Steps

### 1) Add the Todo entity
Create `src/TodoApp.Domain/Todo.cs`:

```csharp
namespace TodoApp.Domain;

public record Todo(
    int Id,
    string Title,
    DateOnly? DueDate,
    bool IsDone = false
);
```

We’re starting with an immutable record. You’ll update items by creating a copy later

## 2) Basic console skeleton
- Open src/TodoApp.Console/Program.cs and add a minimal loop:
```csharp
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

    // explicit type for clarity
    Todo newTodo = new(id++, input, null, false);

    // 'var' is fine when the type is obvious from the RHS:
    var message = $"Created: [{newTodo.Id}] {newTodo.Title}";
    Console.WriteLine(message);
}

Console.WriteLine("Bye!");
```

## 3) Variable choices
- Use explicit types when it improves readability (e.g., `Todo newTodo = ...`).
- Use `var` when the type is clear from the right-hand side.

## 4) Run the Console app
```bash
cd src/TodoApp.Console
dotnet run
```


- From **Visual Studio**:
1. Set `TodoApp.Console` as the startup project and run (right-click).
2. At the top menu, select `Debug > Start Without Debugging` (or press `Ctrl + F5`).

- From **Rider**:
1. Right-click `TodoApp.Console` and select `Run 'TodoApp.Console'`.

## Acceptance Criteria

- Console app lets me type a title and confirms the Todo creation.
- Code compiles; record lives in Domain; console code is simple and readable

## Commit message suggestion

`feat: add Todo record and minimal console I/O`

## PR checklist (student)

- Todo added as a record in Domain.
- Console app creates and prints a Todo.
- I used explicit types where it helped readability.
- I used var only when the type was obvious.