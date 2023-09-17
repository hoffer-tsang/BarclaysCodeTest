using TodoTaskLib.DTOs;
using TodoTaskLib.Enums;

namespace TodoTaskLib.Database
{
    /// <summary>
    /// Act as Data Access Layer to perform CRUD Actions to Database
    /// </summary>
    public class TodoItemService : ITodoItemsService
    {
       /// <summary>
       /// Get all data, or filter search by name, priority or status
       /// </summary>
       /// <param name="name">name of todo item to be filtered</param>
       /// <param name="priority">priority of todo item to be filtered</param>
       /// <param name="status">status of todo item ot be filtered</param>
       /// <returns>GetTodoItems that include total count of TodoItem and all TodoItem</returns>
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

        /// <summary>
        /// Get the TodoItem by Id
        /// </summary>
        /// <param name="id">Id of the todo item</param>
        /// <returns>Todo item if id match, else return null</returns>
        public TodoItem? GetById(int id)
        {
            var task = TodoItemData.Tasks.Where(t => t.Id == id).SingleOrDefault();
            return task;
        }

        /// <summary>
        /// add a todo item to the database
        /// </summary>
        /// <param name="postTask"> a post todo item that is todo item exlcuding id</param>
        /// <returns>the todo item added in</returns>
        /// <exception cref="InvalidDataException">return exception when data is invalid, such as task name is null or repeated.</exception>
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

        /// <summary>
        /// Perform an update to the todoitem in database
        /// </summary>
        /// <param name="id">id of the todo item</param>
        /// <param name="task">the todo item that contains the update value</param>
        /// <returns>the updated todo item</returns>
        /// <exception cref="InvalidDataException">return exception when data is invalid.</exception>
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

        /// <summary>
        /// Remove the data in database by id
        /// </summary>
        /// <param name="id">id of the data to remove</param>
        /// <returns>return true if removed, return false when there is no match in id</returns>
        /// <exception cref="InvalidDataException">return exception when remove task do not have a status of completed.</exception>
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

        /// <summary>
        /// Perform validation for the post data
        /// </summary>
        /// <param name="task">the post data</param>
        /// <returns>true if valid, vice versa </returns>
        private bool ValidatePostData(PostTodoItem task)
        {
            if (TodoItemData.Tasks.Where(t => t.Name == task.Name).Any() || string.IsNullOrWhiteSpace(task.Name))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Perform validation for the put data
        /// </summary>
        /// <param name="id">the id of the data</param>
        /// <param name="task">the put data</param>
        /// <returns>true if valid, vice versa</returns>
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
