namespace TaskManagement.Application.Base.Handler
{
    public class HandlerResponse
    {
        protected HandlerResponse(bool isSuccessful, IEnumerable<string>? errorMessage, Exception? exception)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
            Exception = exception;
        }
        public bool IsSuccessful { get; }
        public IEnumerable<string>? ErrorMessage { get; }
        public Exception? Exception { get; }
        public bool IsUnexpectedFailure => Exception != null;
        public bool IsDomainFailure => !IsSuccessful && Exception == null;
        public static HandlerResponse Success()
        {
            return new HandlerResponse(true, null, null);
        }
        public static HandlerResponse DomainFailure(string errorMessage)
        {
            var errorMessages = new List<string>() { errorMessage };
            return new HandlerResponse(false, errorMessages, null);
        }
        public static HandlerResponse DomainFailure(IEnumerable<string> errorMessages)
        {
            return new HandlerResponse(false, errorMessages, null);
        }
        public static HandlerResponse UnexpectedFailure(Exception exception)
        {
            return new HandlerResponse(false, new List<string>() { exception.Message }, exception);
        }
    }

    public class HandlerResponse<TResponse> : HandlerResponse
    {
        private HandlerResponse(TResponse? response, bool isSuccessful, IEnumerable<string>? errorMessage, Exception? exception): base(isSuccessful, errorMessage, exception)
        {
            Response = response;
        }
        public TResponse? Response { get; }
        public static HandlerResponse<TResponse> Success(TResponse response)
        {
            return new HandlerResponse<TResponse>(response, true, null, null);
        }

        new public static HandlerResponse<TResponse> DomainFailure(IEnumerable<string> errorMessages)
        {
            return new HandlerResponse<TResponse>(default, false, errorMessages, null);
        }
        new public static HandlerResponse<TResponse> DomainFailure(string errorMessage)
        {
            var errorMessages = new List<string>() { errorMessage };
            return new HandlerResponse<TResponse>(default, false, errorMessages, null);
        }


    }
}
