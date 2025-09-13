using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain;


namespace TodoApp.Infrastructure
{
    public class InMemoryTodoStore
    {
        private readonly List<Todo> _items = new();
        private readonly Dictionary<int, Todo> _byId = new();
        private int _nextId = 1;

        public IReadOnlyList<Todo> All => _items;

        public Todo Add(string title, DateOnly? dueDate = null)
        {
            var todo = new Todo(_nextId++, title, dueDate, false);
            _items.Add(todo);
            _byId[todo.Id] = todo;
            return todo;
        }

        public IEnumerable<Todo> List() => _items;

        public bool TryGet(int id, out Todo todo) => _byId.TryGetValue(id, out todo);

        public bool Complete(int id)
        {
            if (!_byId.TryGetValue(id, out var existing)) return false;

            var updated = existing with { IsDone = true };

            var index = _items.FindIndex(t => t.Id == id);
            if (index >= 0) _items[index] = updated;

            _byId[id] = updated;
            return true;
        }

        public bool Delete(int id)
        {
            if (!_byId.Remove(id)) return false;
            var removed = _items.RemoveAll(t => t.Id == id) > 0;
            return removed;
        }

        public void Seed()
        {
            Add("Buy milk", DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
            Add("Finish Module 1 notes", DateOnly.FromDateTime(DateTime.Today.AddDays(2)));
            Add("Call the mechanic");
        }


    }
}
