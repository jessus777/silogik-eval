using SilogikEval.Application.Dtos;

namespace SilogikEval.Application.Interfaces
{
    public interface IContactServiceAsync
    {
        Task<Guid> CreateAsync(CreateContactRequestDto request);

        Task UpdateAsync(UpdateContactRequestDto request);

        Task<ContactResponseDto?> GetByIdAsync(Guid id);

        Task<IEnumerable<ContactResponseDto>> GetAllAsync();
    }
}
