using System.Text.Json;

namespace TaskManagement.Application.Base.Handler
{
    public abstract class BaseHandler<TRequest, TResponse> where TResponse : HandlerResponse
    {
        protected BaseHandler() { }

        public async Task<TResponse> HandleAsync(TRequest command)
        {
            try
            {
                return await BaseHandleAsync(command);
            }
            catch (Exception error)
            {
                var message = $"Unexpected error occurred while handling command of type {typeof(TRequest).FullName}. Command parameters: {JsonSerializer.Serialize(command)}";
                throw new HandlerException(message, error);
            }
        }
        protected abstract Task<TResponse> BaseHandleAsync(TRequest command);
    }
}
