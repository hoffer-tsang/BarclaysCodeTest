
using TodoTaskLib.DTOs;
using TodoTaskLib.Enums;

namespace TodoTaskLib.Database
{
    internal static class TodoItemData
    {
        public static int IdCount = 4;
        public static List<TodoItem> Tasks =
            new List<TodoItem>
            {
                new TodoItem
                {
                    Id = 1,
                    Name = "Test",
                    Priority = 1,
                    Status = Status.NotStarted
                },
                new TodoItem
                {
                    Id = 2,
                    Name = "Test2",
                    Priority = 1,
                    Status = Status.NotStarted
                },
                new TodoItem
                {
                    Id = 3,
                    Name = "Test3",
                    Priority = null,
                    Status = Status.Completed
                },
                new TodoItem
                {
                    Id = 4,
                    Name = "Test4",
                    Priority = 3,
                    Status = null
                },
            };

    }
}
