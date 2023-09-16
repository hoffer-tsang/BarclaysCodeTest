using TodoTaskLib.Database;
using TodoTaskLib.DTOs;
using TodoTaskLib.Enums;

namespace TodoTaskUnitTest
{
    public class TodoItemServiceTest
    {
        private readonly ITodoItemsService _todoItemService;
        private readonly List<TodoItem> _defaultTasks =
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

        public TodoItemServiceTest()
        {
            _todoItemService = new TodoItemService();
        }

        #region Get
        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void TodoItemService_GetAllSet(string name)
        {
            // Arrange
            int? priority = null;
            Status? status = null;

            // Act
            var getTodoItems = _todoItemService.Get(name, priority, status);

            //Assert
            Assert.Equal(4, getTodoItems.Count);
            Assert.NotNull(getTodoItems.Tasks);
            Assert.Collection<TodoItem>(
                getTodoItems.Tasks,
                item => item.Equals(_defaultTasks[0]),
                item => item.Equals(_defaultTasks[1]),
                item => item.Equals(_defaultTasks[2]),
                item => item.Equals(_defaultTasks[3])
                );
        }
        #endregion

        #region GetById
        [Fact]
        public void TodoItemService_GetByIdValidId()
        {
            // Arrange
            int id = 1;

            // Act
            var getByIdTodoItems = _todoItemService.GetById(id);

            //Assert
            Assert.NotNull(getByIdTodoItems);
            Assert.Equivalent(_defaultTasks[0], getByIdTodoItems);
        }

        [Fact]
        public void TodoItemService_GetByIdInvalidId()
        {
            // Arrange
            int id = 999;

            // Act
            var getByIdTodoItems = _todoItemService.GetById(id);

            //Assert
            Assert.Null(getByIdTodoItems);
        }
        #endregion

        #region Add
        [Fact]
        public void TodoItemService_AddSet()
        {
            // Arrange
            var postTask = new PostTodoItem
            {
                Name = "Task5"
            };

            // Act
            var postTodoItems = _todoItemService.Add(postTask);

            //Assert
            Assert.Equal(5, postTodoItems.Id);
            Assert.Equivalent(postTask, postTodoItems);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        [InlineData("Task")]
        public void TodoItemService_AddInvalidName(string name)
        {
            // Arrange
            var postTask = new PostTodoItem
            {
                Name = name
            };

            // Act
            var act = () => _todoItemService.Add(postTask);

            //Assert
            Assert.Throws<InvalidDataException>(act);
        }
        #endregion

        #region Update
        [Fact]
        public void TodoItemService_UpdateSet()
        {
            // Arrange
            var task = new TodoItem
            {
                Id = 3,
                Name = "Task3",
                Priority = 1,
                Status = Status.Completed
            };

            // Act
            var putTodoItems = _todoItemService.Update(3, task);

            //Assert
            Assert.Equivalent(task, putTodoItems);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        [InlineData("Task")]
        public void TodoItemService_UpdateInvalidName(string name)
        {
            // Arrange
            var task = new TodoItem
            {
                Id = 3,
                Name = name
            };

            // Act
            var act = () => _todoItemService.Update(3, task);

            //Assert
            Assert.Throws<InvalidDataException>(act);
        }

        [Fact]
        public void TodoItemService_UpdateIdNotMatch()
        {
            // Arrange
            var task = new TodoItem
            {
                Id = 3,
                Name = "Task5"
            };

            // Act
            var act = () => _todoItemService.Update(999, task);

            //Assert
            Assert.Throws<InvalidDataException>(act);
        }

        [Theory]
        [InlineData(99)]
        [InlineData(0)]
        public void TodoItemService_UpdateInvalid(int id)
        {
            // Arrange
            var task = new TodoItem
            {
                Id = id,
                Name = "Task5"
            };

            // Act
            var putTodoItems = _todoItemService.Update(id, task);

            //Assert
            Assert.Null(putTodoItems);
        }
        #endregion

        #region Remove
        [Fact]
        public void TodoItemService_RemoveSet()
        {
            // Arrange
            var id = 3;

            // Act
            var removeTodoItems = _todoItemService.Remove(id);

            //Assert
            Assert.True(removeTodoItems);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        public void TodoItemService_RemoveStatusNotCompleted(int id)
        {
            // Arrange

            bool result = false;
            Exception? exception = null;

            // Act
            try
            {
                result = _todoItemService.Remove(id);
            }
            catch (InvalidDataException ex)
            {
                exception = ex;
            }

            //Assert
            Assert.IsType<InvalidDataException>(exception);
        }

        [Theory]
        [InlineData(99)]
        [InlineData(0)]
        public void TodoItemService_RemoveInvalidId(int id)
        {
            // Arrange
            
            // Act
            var removeTodoItems = _todoItemService.Remove(id);

            //Assert
            Assert.False(removeTodoItems);
        }
        #endregion
    }
}