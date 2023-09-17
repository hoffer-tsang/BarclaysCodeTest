using TodoTaskLib.Enums;

namespace TodoTaskLib.DTOs
{
    /// <summary>
    /// The standard todo item object
    /// </summary>
    public class TodoItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int? Priority { get; set; }

        public Status? Status { get; set; }

    }
}
