namespace SilogikEval.Application.Exceptions
{
    public class BusinessException
        : Exception
    {
        public string ErrorKey { get; }

        public BusinessException(string errorKey, string fallbackMessage)
            : base(fallbackMessage)
        {
            ErrorKey = errorKey;
        }
    }
}
