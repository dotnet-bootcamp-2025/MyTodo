using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Domain.Phase1
{
    public record Todo(
        int Id,
        string Title,
        DateOnly? DueDate,
        bool IsDone = false
    );
}
