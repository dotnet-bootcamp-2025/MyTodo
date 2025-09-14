using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain;

namespace TodoApp.Infrastructure
{
    public static class RepoSeeder
    {
        public static void Seed(ITodoRepository repo)
        {
            repo.Add("Buy groceries", DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
            repo.Add("Finish the report", DateOnly.FromDateTime(DateTime.Today.AddDays(2)));
            repo.Add("Read a book");
        }
    }
}
