using Microsoft.AspNetCore.Mvc;
using TodoTaskLib.Database;
using TodoTaskLib.DTOs;
using TodoTaskLib.Enums;

namespace TodoTaskAPI.Controllers
{
    [ApiController]
    [Route("api/v1/Task")]
    public class TodoController : ControllerBase
    {
        private ITodoItemsService _todoItemData;

        public TodoController(ITodoItemsService todoItemData)
        {
            _todoItemData = todoItemData;
        }

        [HttpGet]
        public ActionResult<GetTodoItems> Get(string? name = null, int? priority = null, Status? status = null)
        {
           return _todoItemData.Get(name, priority, status);
        }

        [HttpGet("{id}")]
        public ActionResult<TodoItem> Get(int id) 
        {
            var task = _todoItemData.GetById(id);
            return (task is not null) ? task : NotFound();
        }

        [HttpPost]
        public ActionResult<TodoItem> Post(PostTodoItem task)
        {
            try
            {
                return _todoItemData.Add(task);
            }
            catch (InvalidDataException)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public ActionResult<TodoItem> Put(int id, TodoItem task)
        {
            try
            {
                var updatedTask = _todoItemData.Update(id, task);
                return (updatedTask is not null) ? updatedTask : NotFound();
            }
            catch (InvalidDataException)
            {
                return BadRequest();
            }          
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (_todoItemData.Remove(id))
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (InvalidDataException)
            {
                return BadRequest();
            }
        }
    }
}
