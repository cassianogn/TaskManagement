namespace TaskManagement.Domain.Base
{
    public class ValidationResult
    {
        private ValidationResult(IReadOnlyCollection<string> erros)
        {
            Errors = erros;
        }

        public IReadOnlyCollection<string> Errors { get; }

        public bool IsValid => !Errors.Any();

        public static ValidationResult Success()
        {
            return new ValidationResult(Array.Empty<string>());
        }
        public static ValidationResult Failure(IReadOnlyCollection<string> erros)
        {
            return new ValidationResult(erros);
        }
        public static ValidationResult Failure(string error)
        {
            var erros = new List<string> { error };
            return new ValidationResult(erros);
        }
    }
}
