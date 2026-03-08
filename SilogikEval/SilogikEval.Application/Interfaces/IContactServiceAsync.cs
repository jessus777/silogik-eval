using SilogikEval.Application.Entities;

namespace SilogikEval.Application.Interfaces
{
    public interface IContactServiceAsync
    {
        Task<Guid> CreateAsync(Contact contact);

        Task<bool> UpdateAsync(Contact contact);

        Task<bool> DeleteAsync(Guid id);

        Task<Contact?> GetByIdAsync(Guid id);

        Task<IEnumerable<Contact>> GetAllAsync();

        Task<bool> EmailExistsAsync(string email);

    }
}
