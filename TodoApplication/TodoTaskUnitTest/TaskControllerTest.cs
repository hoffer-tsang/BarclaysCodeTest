using TodoTaskLib.Database;
using TodoTaskLib.Enums;
using TodoTaskLib.DTOs;
using Microsoft.AspNetCore.Mvc;
using TodoTaskAPI.Controllers;

namespace TodoTaskUnitTest
{
    public class TaskControllerTest
    {
        private readonly Mock<IDatabaseWrapper> mockDatabase;

        public TaskControllerTest() 
        {
           // mockDatabase = new Mock<IDatabaseWrapper>();
           // mockDatabase.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Status>())).Returns(
           //     new GetTodoItems
           //     {
           //         Tasks = new List<TodoItem>
           //         {
           //             new TodoItem
           //             {
           //                 Id = 1,
           //                 Priority = 1,
           //                 Name = "Task1",
           //                 Status = Status.Completed
           //             }
           //         },
           //         Count = 1
           //     }
           // );

           // mockDatabase.Setup(x => x.Get(It.IsAny<int>())).Returns(
           //         new TodoItem
           //         {
           //             Id = 1,
           //             Priority = 1,
           //             Name = "Task1",
           //             Status = Status.Completed
           //         }
           // );

           // mockDatabase.Setup(x => x.Post(It.IsAny<PostTodoItem>())).Returns(
           //         new TodoItem
           //         {
           //             Id = 1,
           //             Priority = 1,
           //             Name = "Task1",
           //             Status = Status.Completed
           //         }
           // );

           // mockDatabase.Setup(x => x.Put(It.IsAny<int>(), It.IsAny<TodoItem>())).Returns(
           //        new TodoItem
           //        {
           //            Id = 1,
           //            Priority = 1,
           //            Name = "Task1",
           //            Status = Status.Completed
           //        }
           //);

           // mockDatabase.Setup(x => x.Delete(It.IsAny<int>())).Returns(
           //        true
           //);
        }
        

        [Fact]
        public void TaskController_GetTaskSet()
        {
            // Arrange
            var taskController = new TodoController();

            // Act

            ActionResult<GetTodoItems> getTasks = taskController.Get();

            Assert.Equivalent(
                getTasks,
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
                }
                );
        }
    }
}