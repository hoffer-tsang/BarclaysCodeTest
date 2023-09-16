using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoTaskLib.Database;
using TodoTaskLib.DTOs;
using TodoTaskLib.Enums;

namespace TodoTaskAPI.Controllers
{
    [ApiController]
    [Route("api/v1/Task")]
    public class TaskController : ControllerBase
    {
        [HttpGet]
        public ActionResult<GetTasks> Get(string? name = null, int? priority = null, Status? status = null)
        {
            var tasks = InMemoryData.Tasks;

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

            return new GetTasks
            {
                Tasks = tasks,
                Count = tasks.Count
            };
        }

        [HttpGet("{id}")]
        public ActionResult<TodoTaskLib.DTOs.Task> Get(int id) 
        {
            var task = InMemoryData.Tasks.Where(t => t.Id == id).SingleOrDefault();
            if (task == null) {
                return NotFound();
            }
            return task;
        }

        [HttpPost]
        public ActionResult<TodoTaskLib.DTOs.Task> Post(PostTask task)
        {
            if (!ValidatePostData(task))
            {
                return BadRequest();
            }
            return InMemoryData.AddTask(task);
        }

        [HttpPut("{id}")]
        public ActionResult<TodoTaskLib.DTOs.Task> Put(int id, TodoTaskLib.DTOs.Task task)
        {
            if (!ValidatePutData(id, task))
            {
                return BadRequest();
            }

            for (int i = 0; i < InMemoryData.Tasks.Count; i++)
            {
                if (InMemoryData.Tasks[i].Id == task.Id)
                {
                    InMemoryData.Tasks[i].Name = task.Name;
                    InMemoryData.Tasks[i].Priority = task.Priority;
                    InMemoryData.Tasks[i].Status = task.Status;
                    return task;
                }
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            for (int i = 0; i < InMemoryData.Tasks.Count; i++)
            {
                if (InMemoryData.Tasks[i].Id == id)
                {
                    if (InMemoryData.Tasks[i].Status != Status.Completed)
                    {
                        return BadRequest();
                    }
                    InMemoryData.Tasks.RemoveAt(i);
                    return Ok();
                }
            }
            return NotFound();
        }

        private bool ValidatePostData(PostTask task)
        {
            if (InMemoryData.Tasks.Where(t => t.Name == task.Name).Any() || string.IsNullOrWhiteSpace(task.Name))
            {
                return false;
            }
            return true;
        }

        private bool ValidatePutData(int id, TodoTaskLib.DTOs.Task task)
        {
            if (InMemoryData.Tasks.Where(t => t.Name == task.Name).Count() > 1 || 
                id != task.Id || 
                string.IsNullOrWhiteSpace(task.Name))
            {
                return false;
            }
            return true;
        }
    }
}
