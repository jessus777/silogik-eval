namespace SilogikEval.Application.Exceptions
{
    public class AppValidationException
        : Exception
    {
        public IReadOnlyList<ValidationError> Errors { get; }

        public AppValidationException(IEnumerable<ValidationError> errors)
            : base("Se produjeron uno o más errores de validación.")
        {
            Errors = errors.ToList().AsReadOnly();
        }
    }

    public class ValidationError
    {
        public string PropertyName { get; set; } = default!;

        public string ErrorCode { get; set; } = default!;

        public string ErrorMessage { get; set; } = default!;
    }
}
