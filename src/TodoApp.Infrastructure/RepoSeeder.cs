using TodoApp.Domain;
using TodoApp.Domain.Repositories;

namespace TodoApp.Infrastructure;

public static class RepoSeeder
{
    public static void Seed(NewITodoRepository repo)
    {
        repo.Add("Buy milk", DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
        repo.Add("Finish Module 1 notes", DateOnly.FromDateTime(DateTime.Today.AddDays(2)));
        repo.Add("Call the mechanic");
    }
}