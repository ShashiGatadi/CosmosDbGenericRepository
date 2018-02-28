using InfrastructureLayer.Entities.ToDo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Todo
{
    public interface IToDoService
    {
        Task<ToDoItem> CreateToDoItem(ToDoItem toDoItem);
        Task<List<ToDoItem>> GetToDoItems();
    }
}
