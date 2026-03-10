using SilogikEval.Web.Client.Models;

namespace SilogikEval.Web.Client.Services
{
    public interface IApiContactService
    {
        Task<IEnumerable<ContactModel>> GetAllAsync();

        Task<ApiResponseModel<Guid>> CreateAsync(CreateContactModel model);
    }
}
