using System.Data;

namespace SilogikEval.Persistence.Context
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
