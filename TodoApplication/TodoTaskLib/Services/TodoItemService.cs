using TodoTaskLib.DTOs;
using TodoTaskLib.Enums;

namespace TodoTaskLib.Database
{
    public class TodoItemService : ITodoItemsService
    {
       
        public GetTodoItems Get(string? name = null, int? priority = null, Status? status = null)
        {
            var tasks = TodoItemData.Tasks;

            if (!string.IsNullOrWhiteSpace(name))
            {
                tasks = tasks.Where(t => t.Name == name).ToList();
            }

            if (priority != null)
            {
                tasks = tasks.Where(t => t.Priority == priority).ToList();
            }

            if (status != null)
            {
                tasks = tasks.Where(t => t.Status == status).ToList();
            }

            return new GetTodoItems
            {
                Tasks = tasks,
                Count = tasks.Count
            };
        }

        public TodoItem? GetById(int id)
        {
            var task = TodoItemData.Tasks.Where(t => t.Id == id).SingleOrDefault();
            return task;
        }

        public TodoItem Add(PostTodoItem postTask)
        {
            if (!ValidatePostData(postTask))
            {
                throw new InvalidDataException("Task name is null or repeated.");
            }

            TodoItemData.IdCount++;
            var task = new TodoItem
            {
                Id = TodoItemData.IdCount,
                Name = postTask.Name,
                Priority = postTask.Priority,
                Status = postTask.Status
            };
            TodoItemData.Tasks.Add(task);
            return task;
        }

        public TodoItem? Update(int id, TodoItem task)
        {
            if (!ValidatePutData(id, task))
            {
                throw new InvalidDataException("Task name is null or repeated, or Id does not match.");
            }

            for (int i = 0; i < TodoItemData.Tasks.Count; i++)
            {
                if (TodoItemData.Tasks[i].Id == task.Id)
                {
                    TodoItemData.Tasks[i].Name = task.Name;
                    TodoItemData.Tasks[i].Priority = task.Priority;
                    TodoItemData.Tasks[i].Status = task.Status;
                    return task;
                }
            }
            return null;
        }

        public bool Remove(int id)
        {
            for (int i = 0; i < TodoItemData.Tasks.Count; i++)
            {
                if (TodoItemData.Tasks[i].Id == id)
                {
                    if (TodoItemData.Tasks[i].Status != Status.Completed)
                    {
                        throw new InvalidDataException("Status is not completed.");
                    }
                    TodoItemData.Tasks.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        private bool ValidatePostData(PostTodoItem task)
        {
            if (TodoItemData.Tasks.Where(t => t.Name == task.Name).Any() || string.IsNullOrWhiteSpace(task.Name))
            {
                return false;
            }
            return true;
        }

        private bool ValidatePutData(int id, TodoItem task)
        {
            if (TodoItemData.Tasks.Where(t => t.Id != task.Id && t.Name == task.Name).Any() ||
                id != task.Id ||
                string.IsNullOrWhiteSpace(task.Name))
            {
                return false;
            }
            return true;
        }
    }
}
