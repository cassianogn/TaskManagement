using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Base.Handler;
using TaskManagement.Application.TaskItems.Commands.AddTaskItem;
using TaskManagement.Application.TaskItems.Commands.ToggleTaskItem;
using TaskManagement.Application.TaskItems.Queries;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class TaskItemController : ControllerBase
    {
        private readonly ILogger<TaskItemController> _logger;

        public TaskItemController(ILogger<TaskItemController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchKey,
            [FromServices] GetTaskItemsQueryHandler handler, 
            CancellationToken cancellationToken)
        {
            var query = new GetTaskItemsQuery(searchKey);
            var result = await handler.HandleAsync(query, cancellationToken); 
            
            return HandleResult(result, () => Ok(result.Response));
        }

        private IActionResult HandleResult<THandlerResponse>(THandlerResponse result, Func<IActionResult> onActionResult ) where THandlerResponse : HandlerResponse
        {

            if (result.IsUnexpectedFailure)
            {
                _logger.LogError(result.Exception, result.Exception!.Message);
                return StatusCode(500, new { errors = "An unexpected error occurred." });
            }

            if (result.IsDomainFailure)
            {
                return BadRequest(new { errors = result.ErrorMessage });
            }
            return onActionResult();
        }
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] AddTaskItemCommand command,
            [FromServices] AddTaskItemCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.HandleAsync(command, cancellationToken);
            return HandleResult(result, () => CreatedAtAction(
                nameof(GetAll),
                new { id = result.Response },
                result.Response));
        }

        [HttpPatch("{id:guid}/toggle")]
        public async Task<IActionResult> ToggleStatus(
            Guid id,
            [FromServices] ToggleTaskItemCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new ToggleTaskItemCommand(id);
            var result = await handler.HandleAsync(command, cancellationToken);

            return HandleResult(result, () => NoContent());
        }
    }
}
