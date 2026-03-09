using System.Data;
using Dapper;
using SilogikEval.Application.Entities;
using SilogikEval.Application.Interfaces;
using SilogikEval.Persistence.Context;

namespace SilogikEval.Persistence.Repositories
{
    public class TranslationRepositoryAsync
        : ITranslationRepositoryAsync
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TranslationRepositoryAsync(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Translation>> GetByLanguageCodeAsync(string languageCode)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<Translation>(
                "usp_Translation_GetByLanguage",
                new { LanguageCode = languageCode },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Language>> GetActiveLanguagesAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<Language>(
                "usp_Language_GetActive",
                commandType: CommandType.StoredProcedure);
        }
    }
}
