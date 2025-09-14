namespace TodoApp.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Services;
//using TodoApp.Domain.Repositories; commented out to fix CS0246, "Because I'm using a new ITodoRepository.cs file"
using TodoApp.Infrastructure.Repositories;
using TodoApp.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
        services.AddScoped<ITodoService, TodoService>();

        return services;
    }
}
