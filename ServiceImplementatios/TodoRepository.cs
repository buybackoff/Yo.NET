using System.Collections.Generic;
using System.Linq;
using Contracts.ServiceModels;

namespace ServiceImplementations {
    public class TodoRepository {
        readonly List<Todo> _todos = new List<Todo>();

        public List<Todo> GetByIds(long[] ids) {
            return _todos.Where(x => ids.Contains(x.Id)).ToList();
        }

        public List<Todo> GetAll() {
            return _todos;
        }

        public Todo Store(Todo todo) {
            var existing = _todos.FirstOrDefault(x => x.Id == todo.Id);
            if (existing == null) {
                var newId = _todos.Count > 0 ? _todos.Max(x => x.Id) + 1 : 1;
                todo.Id = newId;
            }
            _todos.Add(todo);
            return todo;
        }

        public void DeleteByIds(params long[] ids) {
            _todos.RemoveAll(x => ids.Contains(x.Id));
        }
    }
}