
namespace TodoTaskLib.DTOs
{
    /// <summary>
    /// Data transfer object that hold the structure when a list of todo items is returned
    /// </summary>
    public class TodoItemsGetResult
    {
        public List<TodoItem>? Tasks { get; set; }

        public int Count { get; set; }

    }
}
