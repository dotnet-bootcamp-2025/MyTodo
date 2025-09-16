namespace TodoApp.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Interfaces;
using TodoApp.Application;
using TodoApp.Domain.Repositories;
using TodoApp.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
       // services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
       // services.AddScoped<ITodoService, TodoService>();
        
        return services;
    }
}