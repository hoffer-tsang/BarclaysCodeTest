using Microsoft.AspNetCore.Mvc;
using TodoTaskLib.Database;
using TodoTaskLib.DTOs;
using TodoTaskLib.Enums;

namespace TodoTaskAPI.Controllers
{
    /// <summary>
    /// Todo controller as front end layer to the API
    /// </summary>
    [ApiController]
    [Route("api/v1/Task")]
    public class TodoController : ControllerBase
    {
        private ITodoItemsService _todoItemData;

        public TodoController(ITodoItemsService todoItemData)
        {
            _todoItemData = todoItemData;
        }

        /// <summary>
        /// Get request that can get all request or filter by name priority and status
        /// </summary>
        /// <param name="name">name to be filtered</param>
        /// <param name="priority">priority to be filtered</param>
        /// <param name="status">status to be filtered</param>
        /// <returns>a get todo items that include a list of matching item and the total number of it</returns>
        [HttpGet]
        public ActionResult<TodoItemsGetResult> Get(string? name = null, int? priority = null, Status? status = null)
        {
            var tasks = _todoItemData.Get(name, priority, status);
            return new TodoItemsGetResult
            {
                Tasks = tasks,
                Count = tasks.Count
            };
        }

        /// <summary>
        /// Get request by id that query the todo item with the specific id
        /// </summary>
        /// <param name="id">id of the todo item</param>
        /// <returns> return todo item or not found when id do not exists </returns>
        [HttpGet("{id}")]
        public ActionResult<TodoItem> Get(int id) 
        {
            var task = _todoItemData.GetById(id);
            return (task is not null) ? task : NotFound();
        }

        /// <summary>
        /// Post request that add the item to the database
        /// </summary>
        /// <param name="task"> the item to be added</param>
        /// <returns>the added todo item with id, or bad request if data is invalid</returns>
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

        /// <summary>
        /// Put request that update the item to the database
        /// </summary>
        /// <param name="id"> id of the item to be updated </param>
        /// <param name="task"> the item to be updated </param>
        /// <returns>the updated item, or not found when id do not exists, or bad request with invalid data</returns>
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

        /// <summary>
        /// Delete request that delete the item by id
        /// </summary>
        /// <param name="id">id of the item</param>
        /// <returns>Ok if deleted, not found if id is not found, and bad request if status is not compelted </returns>
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
