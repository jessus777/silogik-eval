using SilogikEval.Application.Entities;
using SilogikEval.Application.Responses;

namespace SilogikEval.Application.Interfaces
{
    public interface IContactRepositoryAsync
    {
        Task<Guid> CreateAsync(Contact contact);

        Task UpdateAsync(Contact contact);

        Task<Contact?> GetByIdAsync(Guid id);

        Task<PagedResult<Contact>> GetAllAsync(int pageNumber, int pageSize, string? search = null);

        Task DeleteAsync(Guid id);

        Task<bool> EmailExistsAsync(string email);
    }
}
