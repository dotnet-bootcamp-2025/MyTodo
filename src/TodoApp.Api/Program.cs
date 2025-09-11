using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Infrastructure;
//Phase 0 setup complete
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Define API endpoints
app.MapGet("/todos", async (ITodoService todoService) =>
{
    var todos = await todoService.GetAllTodosAsync();
    return Results.Ok(todos);
})
.WithName("GetAllTodos")
.WithOpenApi();

app.MapGet("/todos/{id}", async (Guid id, ITodoService todoService) =>
{
    var todo = await todoService.GetTodoByIdAsync(id);
    return todo != null ? Results.Ok(todo) : Results.NotFound();
})
.WithName("GetTodoById")
.WithOpenApi();

app.MapPost("/todos", async (CreateTodoDto createTodoDto, ITodoService todoService) =>
{
    try
    {
        var todo = await todoService.CreateTodoAsync(createTodoDto);
        return Results.Created($"/todos/{todo.Id}", todo);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("CreateTodo")
.WithOpenApi();

app.MapPut("/todos/{id}", async (Guid id, UpdateTodoDto updateTodoDto, ITodoService todoService) =>
{
    try
    {
        var todo = await todoService.UpdateTodoAsync(id, updateTodoDto);
        return todo != null ? Results.Ok(todo) : Results.NotFound();
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("UpdateTodo")
.WithOpenApi();

app.MapDelete("/todos/{id}", async (Guid id, ITodoService todoService) =>
{
    var result = await todoService.DeleteTodoAsync(id);
    return result ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteTodo")
.WithOpenApi();

app.MapPost("/todos/{id}/complete", async (Guid id, ITodoService todoService) =>
{
    var result = await todoService.CompleteTodoAsync(id);
    return result ? Results.Ok() : Results.NotFound();
})
.WithName("CompleteTodo")
.WithOpenApi();

app.MapPost("/todos/{id}/reset", async (Guid id, ITodoService todoService) =>
{
    var result = await todoService.ResetTodoAsync(id);
    return result ? Results.Ok() : Results.NotFound();
})
.WithName("ResetTodo")
.WithOpenApi();

app.Run();