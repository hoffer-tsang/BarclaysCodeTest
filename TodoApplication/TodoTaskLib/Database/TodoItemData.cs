
using TodoTaskLib.DTOs;
using TodoTaskLib.Enums;

namespace TodoTaskLib.Database
{
    /// <summary>
    /// In memory collection that work as database
    /// </summary>
    internal static class TodoItemData
    {
        public static int IdCount = 4;
        public static List<TodoItem> Tasks =
            new List<TodoItem>
            {
                new TodoItem
                {
                    Id = 1,
                    Name = "Task",
                    Priority = 1,
                    Status = Status.NotStarted
                },
                new TodoItem
                {
                    Id = 2,
                    Name = "Task2",
                    Priority = 1,
                    Status = Status.NotStarted
                },
                new TodoItem
                {
                    Id = 3,
                    Name = "Task3",
                    Priority = null,
                    Status = Status.Completed
                },
                new TodoItem
                {
                    Id = 4,
                    Name = "Task4",
                    Priority = 3,
                    Status = null
                },
            };

    }
}
