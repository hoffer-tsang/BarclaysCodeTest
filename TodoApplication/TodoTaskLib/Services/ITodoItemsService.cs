
using TodoTaskLib.DTOs;
using TodoTaskLib.Enums;

namespace TodoTaskLib.Database
{
    public interface ITodoItemsService
    {
        GetTodoItems Get(string? name, int? priority, Status? status);
        TodoItem? GetById(int id);
        TodoItem Add(PostTodoItem postTask);
        TodoItem? Update(int id, TodoItem task);
        bool Remove(int id);


    }
}
