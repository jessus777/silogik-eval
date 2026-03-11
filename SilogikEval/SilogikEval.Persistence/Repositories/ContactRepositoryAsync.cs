using System.Data;
using Dapper;
using SilogikEval.Application.Entities;
using SilogikEval.Application.Interfaces;
using SilogikEval.Application.Responses;
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

        public async Task<PagedResult<Contact>> GetAllAsync(int pageNumber, int pageSize, string? search = null)
        {
            using var connection = _connectionFactory.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                "usp_Contact_GetAll",
                new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Search = search
                },
                commandType: CommandType.StoredProcedure);

            var totalCount = await multi.ReadSingleAsync<int>();
            var contacts = await multi.ReadAsync<Contact>();

            return PagedResult<Contact>.Create(contacts, pageNumber, pageSize, totalCount);
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                "usp_Contact_Delete",
                new { Id = id },
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
