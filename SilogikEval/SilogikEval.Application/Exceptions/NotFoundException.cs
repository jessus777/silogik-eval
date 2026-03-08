using SilogikEval.Application.Constants;

namespace SilogikEval.Application.Exceptions
{
    public class NotFoundException
        : Exception
    {
        public string ErrorKey { get; } = ErrorKeys.EntityNotFound;

        public string EntityName { get; }

        public object Key { get; }

        public NotFoundException(string entityName, object key)
            : base($"La entidad \"{entityName}\" con identificador ({key}) no fue encontrada.")
        {
            EntityName = entityName;
            Key = key;
        }
    }
}
