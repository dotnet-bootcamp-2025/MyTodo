using TodoApp.Domain;

namespace TodoApp.Infrastructure;

public static class RepoSeeder
{
    public static void Seed(ITodoRepository repo)
    {
        repo.Add("Buy cream", DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
        repo.Add("Finish module 1 exercises", DateOnly.FromDateTime(DateTime.Today.AddDays(2)));
        repo.Add("Feed the cat");
    }
}

