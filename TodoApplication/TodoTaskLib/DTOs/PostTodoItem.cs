
using TodoTaskLib.Enums;

namespace TodoTaskLib.DTOs
{
    /// <summary>
    /// Data transfer object for posting the todo item
    /// </summary>
    public class PostTodoItem
    {
        public string Name { get; set; } = string.Empty;

        public int? Priority { get; set; }

        public Status? Status { get; set; }
    }
}
