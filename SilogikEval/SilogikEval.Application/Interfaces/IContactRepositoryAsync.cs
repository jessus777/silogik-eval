using SilogikEval.Application.Entities;

namespace SilogikEval.Application.Interfaces
{
    public interface IContactRepositoryAsync
    {
        Task<Guid> CreateAsync(Contact contact);

        Task UpdateAsync(Contact contact);

        Task<Contact?> GetByIdAsync(Guid id);

        Task<IEnumerable<Contact>> GetAllAsync();

        Task<bool> EmailExistsAsync(string email);
    }
}
