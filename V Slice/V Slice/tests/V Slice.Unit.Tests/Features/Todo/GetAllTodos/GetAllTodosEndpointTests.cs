using FastEndpoints;
using FluentAssertions;
using NSubstitute;
using V_Slice.Features.Todo;
using V_Slice.Features.Todo.GetAllTodos;

namespace V_Slice.Unit.Tests.Features.Todo.GetAllTodos
{
    public class GetAllTodosEndpointTests
    {
        [Fact]
        public async Task GetAllTodos_WithData_ReturnsItems()
        {
            // Arrange
            var items = new[]
            {
            new TodoEntity
            {
                Id = Guid.NewGuid(),
                Text = "My todo item"
            }
        };

            var repo = Substitute.For<ITodoRepository>();
            repo.GetAllAsync(Arg.Any<CancellationToken>())
                .Returns(x => Task.FromResult<IEnumerable<TodoEntity>>(items));

            var endpoint = Factory.Create<GetAllTodosEndpoint>(repo);

            // Act
            await endpoint.HandleAsync(default);
            var response = endpoint.Response;

            response.Should().NotBeNullOrEmpty();
        }

    }
}