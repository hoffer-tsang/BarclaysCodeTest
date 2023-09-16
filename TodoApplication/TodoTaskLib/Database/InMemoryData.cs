using TodoTaskLib.DTOs;
using TodoTaskLib.Enums;

namespace TodoTaskLib.Database
{
    public static class InMemoryData
    {
        private static int IdCount = 4;
        public static List<DTOs.Task> Tasks { get; set; } = 
            new List<DTOs.Task>
            {
                new DTOs.Task
                {
                    Id = 1,
                    Name = "Test",
                    Priority = 1,
                    Status = Status.NotStarted
                },
                new DTOs.Task
                {
                    Id = 2,
                    Name = "Test2",
                    Priority = 1,
                    Status = Status.NotStarted
                },
                new DTOs.Task
                {
                    Id = 3,
                    Name = "Test3",
                    Priority = null,
                    Status = Status.Completed
                },
                new DTOs.Task
                {
                    Id = 4,
                    Name = "Test4",
                    Priority = 3,
                    Status = null
                },
            };

        public static DTOs.Task AddTask (PostTask postTask)
        {
            IdCount++;
            var task = new DTOs.Task
            {
                Id = IdCount,
                Name = postTask.Name,
                Priority = postTask.Priority,
                Status = postTask.Status
            };
            Tasks.Add(task);
            return task;
        }
    }
}
