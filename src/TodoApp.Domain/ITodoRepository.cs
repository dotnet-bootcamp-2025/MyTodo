using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Domain;

public interface ITodoRepository
{
    IReadOnlyList<Todo> All { get; }

    Todo Add(string title, DateOnly? dueDate = null);

    bool TryGet(int id, out Todo todo);

    bool Complete(int id);

    bool Toggle(int id);

    bool Delete(int id);
}

