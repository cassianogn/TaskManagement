
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TaskManagement.Application.Base.Handler;
using TaskManagement.Application.TaskItems.Commands.AddTaskItem;
using TaskManagement.Application.TaskItems.Queries;
using TaskManagement.Domain.TaskItems;
using TaskManagement.Infrastructure;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Tests.TaskItemTests
{
    public class TaskItemHandlersTest : IDisposable
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly ServiceProvider _serviceProvider;
        private readonly IServiceScope _scope;

        public TaskItemHandlersTest()
        {
            var taskItems = new List<TaskItem>();
            _serviceProvider = new ServiceCollection().AddInfrastructure().BuildServiceProvider();
            _scope = _serviceProvider.CreateScope();
            _taskItemRepository = _scope.ServiceProvider.GetRequiredService<ITaskItemRepository>();

        }

        [Fact]
        public async Task HandleTaskItem_ShouldAdd_RunWitSuccess()
        {
            // Arrange
            var addCommand = new AddTaskItemCommand("Test Task");
            var addHandler = new AddTaskItemCommandHandler(_taskItemRepository);
            // Act
            HandlerResponse<Guid> result = await addHandler.HandleAsync(addCommand);
            // Assert
            Assert.NotNull(result);

            // arrange 
            var command = new GetTaskItemsQuery(addCommand.Title);
            var handler = new GetTaskItemsQueryHandler(_taskItemRepository);
            // Act
            HandlerResponse<IReadOnlyCollection<GetTaskItemsQueryResult>> queryResult = await handler.HandleAsync(command);
            // Assert
            Assert.NotNull(queryResult);
            Assert.NotNull(queryResult.Response);
            Assert.True(queryResult.IsSuccessful);
            Assert.Contains(queryResult.Response, item => item.Title.Contains(addCommand.Title));
        }

        public void Dispose()
        {
            var context = _scope.ServiceProvider.GetRequiredService<TaskManagementDbContext>();
            context.Database.EnsureDeleted();
            _scope.Dispose();
            _serviceProvider.Dispose();
        }

    }
}