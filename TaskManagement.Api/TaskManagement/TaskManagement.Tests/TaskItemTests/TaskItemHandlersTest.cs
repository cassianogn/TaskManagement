
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TaskManagement.Application;
using TaskManagement.Application.Base.Handler;
using TaskManagement.Application.TaskItems.Commands.AddTaskItem;
using TaskManagement.Application.TaskItems.Commands.ToggleTaskItem;
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
            _serviceProvider = new ServiceCollection()
                .AddApplication()
                .AddInfrastructure()
                .BuildServiceProvider();
            _scope = _serviceProvider.CreateScope();
            _taskItemRepository = _scope.ServiceProvider.GetRequiredService<ITaskItemRepository>();

        }

        [Fact]
        public async Task HandleTaskItem_ShouldAdd_RunWitSuccess()
        {
            // Arrange
            var addCommand = new AddTaskItemCommand("Test Task");
            var addHandler = _scope.ServiceProvider.GetRequiredService<AddTaskItemCommandHandler>();
            // Act
            HandlerResponse<Guid> result = await addHandler.HandleAsync(addCommand, default);
            // Assert
            Assert.NotNull(result);

            // arrange 
            var command = new GetTaskItemsQuery(addCommand.Title);
            var handler = _scope.ServiceProvider.GetRequiredService<GetTaskItemsQueryHandler>();
            // Act
            HandlerResponse<IReadOnlyCollection<GetTaskItemsQueryResult>> queryResult = await handler.HandleAsync(command, default);
            // Assert
            Assert.NotNull(queryResult);
            Assert.NotNull(queryResult.Response);
            Assert.True(queryResult.IsSuccessful);
            Assert.Contains(queryResult.Response, item => item.Title.Contains(addCommand.Title));
        }

        [Fact]
        public async Task HandleTaskItem_ShouldToggle_RunWIthSuccess()
        {
            // Arrange
            var queryHandler = _scope.ServiceProvider.GetRequiredService<GetTaskItemsQueryHandler>();
            var addCommand = new AddTaskItemCommand("Test Task for Toggle");
            var addHandler = _scope.ServiceProvider.GetRequiredService<AddTaskItemCommandHandler>();
            // Act
            HandlerResponse<Guid> result = await addHandler.HandleAsync(addCommand, default);
            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccessful);
         
            // arrange
            var toggleCommand = new ToggleTaskItemCommand(result.Response);
            var toggleHandler = _scope.ServiceProvider.GetRequiredService<ToggleTaskItemCommandHandler>();
            Guid newTaskItemId = result.Response;
            // Act
            HandlerResponse toggleResult = await toggleHandler.HandleAsync(toggleCommand, default);
            // Assert
            Assert.NotNull(toggleResult);
            Assert.True(toggleResult.IsSuccessful);
            Assert.Null(toggleResult.ErrorMessage);
            
            var finalQueryCommand = new GetTaskItemsQuery(addCommand.Title);
            HandlerResponse<IReadOnlyCollection<GetTaskItemsQueryResult>> finalQueryResult = await queryHandler.HandleAsync(finalQueryCommand, default);
            Assert.True(finalQueryResult.IsSuccessful);
            Assert.NotNull(finalQueryResult);
            Assert.NotNull(finalQueryResult.Response);
            var toggledTAsk = finalQueryResult.Response.FirstOrDefault(taskItem => taskItem.Id == newTaskItemId);
            Assert.NotNull(toggledTAsk);
            Assert.True(toggledTAsk!.IsCompleted, "Task should be marked as completed, but does not.");
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