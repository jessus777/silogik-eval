using System.Data;
using Dapper;
using SilogikEval.Application.Entities;
using SilogikEval.Application.Interfaces;
using SilogikEval.Persistence.Context;

namespace SilogikEval.Persistence.Repositories
{
    public class ContactRepositoryAsync
        : IContactRepositoryAsync
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ContactRepositoryAsync(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Guid> CreateAsync(Contact contact)
        {
            using var connection = _connectionFactory.CreateConnection();

            var id = await connection.ExecuteScalarAsync<Guid>(
                "usp_Contact_Insert",
                new
                {
                    contact.Id,
                    contact.Email,
                    contact.FirstName,
                    contact.SecondName,
                    contact.LastName,
                    contact.SecondLastName,
                    contact.Comments,
                    contact.FilePath,
                    contact.CreatedDate,
                    contact.LastModifiedDate
                },
                commandType: CommandType.StoredProcedure);

            return id;
        }

        public async Task UpdateAsync(Contact contact)
        {
            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                "usp_Contact_Update",
                new
                {
                    contact.Id,
                    contact.FirstName,
                    contact.SecondName,
                    contact.LastName,
                    contact.SecondLastName,
                    contact.Comments,
                    contact.FilePath,
                    contact.LastModifiedDate
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Contact?> GetByIdAsync(Guid id)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Contact>(
                "usp_Contact_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<Contact>(
                "usp_Contact_GetAll",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            using var connection = _connectionFactory.CreateConnection();

            var count = await connection.ExecuteScalarAsync<int>(
                "usp_Contact_EmailExists",
                new { Email = email },
                commandType: CommandType.StoredProcedure);

            return count > 0;
        }
    }
}
