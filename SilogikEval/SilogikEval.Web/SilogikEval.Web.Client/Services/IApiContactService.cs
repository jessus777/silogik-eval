using SilogikEval.Web.Client.Models;

namespace SilogikEval.Web.Client.Services
{
    public interface IApiContactService
    {
        Task<IEnumerable<ContactModel>> GetAllAsync();

        Task<ContactModel?> GetByIdAsync(Guid id);

        Task<ApiResponseModel<Guid>> CreateAsync(CreateContactModel model);

        Task<ApiResponseModel<object>> UpdateAsync(UpdateContactModel model);
    }
}
