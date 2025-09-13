using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Domain
{
    public record TodoOld(int Id, string Title, DateOnly? DueDate, bool IsDone = false);
    
}
