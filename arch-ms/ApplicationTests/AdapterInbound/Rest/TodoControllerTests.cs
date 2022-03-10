using Application.AdapterInbound.Rest;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationTests.AdapterInbound.Rest
{
    public class TodoControllerTests
    {

        [Fact]
        public async Task When_HTTPGetTodo_Expect_Todo()
        {
            // Arrange
            var service = new Mock<TodoService>();
            var controller = new TodoController();

            // Act
            var actionResult = await controller.Get(service.Object, "1212");
            var result = actionResult.Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
        }
    }
}
