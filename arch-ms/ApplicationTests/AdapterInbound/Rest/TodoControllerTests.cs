using Application.AdapterInbound.Rest;
using Microsoft.AspNetCore.Mvc;
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
            var controller = new TodoController();

            // Act
            var actionResult = await controller.Get();
            var result = actionResult.Result as OkObjectResult;

            // Assert
            Assert.Equal("", result.Value);
        }
    }
}
