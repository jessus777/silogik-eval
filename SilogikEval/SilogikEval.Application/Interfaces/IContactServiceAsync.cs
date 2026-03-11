using SilogikEval.Application.Dtos;
using SilogikEval.Application.Responses;

namespace SilogikEval.Application.Interfaces
{
    public interface IContactServiceAsync
    {
        Task<Guid> CreateAsync(CreateContactRequestDto request);

        Task UpdateAsync(UpdateContactRequestDto request);

        Task<ContactResponseDto?> GetByIdAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<PagedResult<ContactResponseDto>> GetAllAsync(int pageNumber, int pageSize, string? search = null);
    }
}
