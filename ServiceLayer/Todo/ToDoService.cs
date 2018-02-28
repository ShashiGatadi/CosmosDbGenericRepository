using InfrastructureLayer.Data;
using InfrastructureLayer.Entities.ToDo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Todo
{
    public class ToDoService : IToDoService
    {
        private IRepository<ToDoItem> _repository;

        public ToDoService(IRepository<ToDoItem> repository)
        {
            this._repository = repository;
        }

        public async Task<ToDoItem> CreateToDoItem(ToDoItem toDoItem)
        {
            toDoItem.CreatedOnUtc = DateTime.UtcNow;
            return await _repository.CreateItemAsync(toDoItem);
        }

        public async Task<List<ToDoItem>> GetToDoItems()
        {
            var result =  await _repository.GetItemsAsync();
            return result.ToList();
        }
    }
}
