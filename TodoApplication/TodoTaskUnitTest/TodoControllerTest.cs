using TodoTaskLib.Database;
using TodoTaskLib.Enums;
using TodoTaskLib.DTOs;
using Microsoft.AspNetCore.Mvc;
using TodoTaskAPI.Controllers;

namespace TodoTaskUnitTest
{
    public class TodoControllerTest
    {
        private readonly Mock<ITodoItemsService> _mockTodoItemService;
        private readonly TodoController _taskController;

        public TodoControllerTest() 
        {
            
            _mockTodoItemService = new Mock<ITodoItemsService>();
            _mockTodoItemService.Setup(x => x.Get(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<Status?>())).Returns(
               new GetTodoItems
               {
                   Tasks = new List<TodoItem>
                        {
                            new TodoItem
                            {
                                Id = 1,
                                Priority = 1,
                                Name = "Task1",
                                Status = Status.Completed
                            }
                        },
                   Count = 1
               });

            _mockTodoItemService.Setup(x => x.GetById(999)).Returns(null as TodoItem);

            _mockTodoItemService.Setup(x => x.GetById(1)).Returns(
                    new TodoItem
                    {
                        Id = 1,
                        Priority = 1,
                        Name = "Task1",
                        Status = Status.Completed
                    }
            );

            _mockTodoItemService.Setup(x => x.Add(It.Is<PostTodoItem>(p => p.Name == "Valid"))).Returns(
                    new TodoItem
                    {
                        Id = 1,
                        Priority = 1,
                        Name = "Task1",
                        Status = Status.Completed
                    }
            );

            _mockTodoItemService.Setup(x => x.Add(It.Is<PostTodoItem>(p => p.Name == "Invalid"))).Throws(
                new InvalidDataException("")
           );

            _mockTodoItemService.Setup(x => x.Update(It.IsAny<int>(), It.Is<TodoItem>(p => p.Name == "Valid"))).Returns(
                   new TodoItem
                   {
                       Id = 1,
                       Priority = 1,
                       Name = "Task1",
                       Status = Status.Completed
                   }
           );

            _mockTodoItemService.Setup(x => x.Update(It.IsAny<int>(), It.Is<TodoItem>(p => p.Name == "InvalidId"))).Returns(null as TodoItem);

            _mockTodoItemService.Setup(x => x.Update(It.IsAny<int>(), It.Is<TodoItem>(p => p.Name == "InvalidName"))).Throws(
                new InvalidDataException("")
           );


            _mockTodoItemService.Setup(x => x.Remove(1)).Returns(
                   true
           );
            _mockTodoItemService.Setup(x => x.Remove(0)).Throws(
                new InvalidDataException("")
           );
            _mockTodoItemService.Setup(x => x.Remove(999)).Returns(
                   false
           );

            _taskController = new TodoController(_mockTodoItemService.Object);
        }

        #region Get Request
        [Fact]
        public void TodoController_GetSet()
        {
            // Arrange
            

            // Act
            var getTodoItems = _taskController.Get(null, null, null);

            //Assert
            Assert.Null(getTodoItems.Result);
            Assert.Equivalent(
                new GetTodoItems
                {
                    Tasks = new List<TodoItem>
                    {
                        new TodoItem
                        {
                            Id = 1,
                            Priority = 1,
                            Name = "Task1",
                            Status = Status.Completed
                        }
                    },
                    Count = 1
                },
                getTodoItems.Value);
        }

        [Fact]
        public void TodoController_GetByIdSet()
        {
            // Arrange
            var id = 1;

            // Act
            var getTodoItem = _taskController.Get(id);

            //Assert
            Assert.Null(getTodoItem.Result);
            Assert.Equivalent(
                new TodoItem
                {
                    Id = 1,
                    Priority = 1,
                    Name = "Task1",
                    Status = Status.Completed
                }, 
                getTodoItem.Value);
        }

        [Fact]
        public void TodoController_GetByIdSetInvalidId_NotFound()
        {
            // Arrange
            var id = 999;

            // Act
            var getTodoItem = _taskController.Get(id);

            //Assert
            Assert.Equivalent(new NotFoundResult(), getTodoItem.Result);
            Assert.Null(getTodoItem.Value);
        }

        #endregion

        #region Post Request
        [Fact]
        public void TodoController_PostSet()
        {
            // Arrange
            var postTodoItem = new PostTodoItem { Name = "Valid" };

            // Act
            var postResult = _taskController.Post(postTodoItem);

            //Assert
            Assert.Null(postResult.Result);
            Assert.Equivalent(
                new TodoItem
                {
                    Id = 1,
                    Priority = 1,
                    Name = "Task1",
                    Status = Status.Completed
                },
                postResult.Value);
        }

        [Fact]
        public void TodoController_PostSetInvalidId_BadRequest()
        {
            // Arrange
            var postTodoItem = new PostTodoItem { Name = "Invalid" };

            // Act
            var postResult = _taskController.Post(postTodoItem);

            //Assert
            Assert.Equivalent(new BadRequestResult(), postResult.Result);
            Assert.Null(postResult.Value);
        }
        #endregion

        #region Put Request
        [Fact]
        public void TodoController_PutSet()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Name = "Valid" };
            var id = 1;

            // Act
            var putResult = _taskController.Put(id, todoItem);

            //Assert
            Assert.Null(putResult.Result);
            Assert.Equivalent(
                new TodoItem
                {
                    Id = 1,
                    Priority = 1,
                    Name = "Task1",
                    Status = Status.Completed
                },
                putResult.Value);
        }

        [Fact]
        public void TodoController_PutSetInvalidName_BadRequest()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Name = "InvalidName" };
            var id = 1;

            // Act
            var putResult = _taskController.Put(id, todoItem);

            //Assert
            Assert.Equivalent(new BadRequestResult(), putResult.Result);
            Assert.Null(putResult.Value);
        }

        [Fact]
        public void TodoController_PutSetInvalidId_NotFound()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Name = "InvalidId" };
            var id = 1;

            // Act
            var putResult = _taskController.Put(id, todoItem);

            //Assert
            Assert.Equivalent(new NotFoundResult(), putResult.Result);
            Assert.Null(putResult.Value);
        }
        #endregion

        #region Delete Request
        [Fact]
        public void TodoController_DeleteSet()
        {
            // Arrange
            var id = 1;

            // Act
            var deleteResult = _taskController.Delete(id);

            //Assert
            Assert.Equivalent(new OkResult(), deleteResult);
        }

        [Fact]
        public void TodoController_DeleteSetInvalidStatus_BadRequest()
        {
            // Arrange
            var id = 0;

            // Act
            var deleteResult = _taskController.Delete(id);

            //Assert
            Assert.Equivalent(new BadRequestResult(), deleteResult);
        }

        [Fact]
        public void TodoController_DeleteSetInvalidId_NotFound()
        {
            // Arrange
            var id = 999;

            // Act
            var deleteResult = _taskController.Delete(id);

            //Assert
            Assert.Equivalent(new NotFoundResult(), deleteResult);
        }
        #endregion
    }
}