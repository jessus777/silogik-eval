using SilogikEval.Web.Client.Models;

namespace SilogikEval.Web.Client.Services
{
    public interface IApiContactService
    {
        Task<PagedResultModel<ContactModel>> GetAllAsync(int page = 1, int pageSize = 10, string? search = null);

        Task<ContactModel?> GetByIdAsync(Guid id);

        Task<ApiResponseModel<Guid>> CreateAsync(CreateContactModel model);

        Task<ApiResponseModel<object>> UpdateAsync(UpdateContactModel model);

        Task<ApiResponseModel<object>> DeleteAsync(Guid id);
    }
}
