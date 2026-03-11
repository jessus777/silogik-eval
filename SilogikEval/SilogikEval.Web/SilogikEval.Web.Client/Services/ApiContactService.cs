using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SilogikEval.Web.Client.Models;

namespace SilogikEval.Web.Client.Services
{
    public class ApiContactService
        : IApiContactService
    {
        private readonly HttpClient _httpClient;
        private readonly ILanguageStateService _languageState;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiContactService(HttpClient httpClient, ILanguageStateService languageState)
        {
            _httpClient = httpClient;
            _languageState = languageState;
        }

        public async Task<PagedResultModel<ContactModel>> GetAllAsync(int page = 1, int pageSize = 10, string? search = null)
        {
            var url = $"api/contacts?page={page}&pageSize={pageSize}";

            if (!string.IsNullOrWhiteSpace(search))
                url += $"&search={Uri.EscapeDataString(search)}";

            HttpResponseMessage httpResponse;

            try
            {
                httpResponse = await _httpClient.GetAsync(url);
            }
            catch
            {
                return new PagedResultModel<ContactModel>();
            }

            if (!httpResponse.IsSuccessStatusCode)
                return new PagedResultModel<ContactModel>();

            var json = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<ApiResponseModel<PagedResultModel<ContactModel>>>(json, JsonOptions);

            return response?.Data ?? new PagedResultModel<ContactModel>();
        }

        public async Task<ContactModel?> GetByIdAsync(Guid id)
        {
            HttpResponseMessage httpResponse;

            try
            {
                httpResponse = await _httpClient.GetAsync($"api/contacts/{id}");
            }
            catch
            {
                return null;
            }

            if (!httpResponse.IsSuccessStatusCode)
                return null;

            var json = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<ApiResponseModel<ContactModel>>(json, JsonOptions);

            return response?.Data;
        }

        public async Task<ApiResponseModel<Guid>> CreateAsync(CreateContactModel model)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(model.Email), "Email");
            content.Add(new StringContent(model.FirstName), "FirstName");
            content.Add(new StringContent(model.LastName), "LastName");
            content.Add(new StringContent(model.Comments), "Comments");

            if (!string.IsNullOrEmpty(model.SecondName))
                content.Add(new StringContent(model.SecondName), "SecondName");

            if (!string.IsNullOrEmpty(model.SecondLastName))
                content.Add(new StringContent(model.SecondLastName), "SecondLastName");

            if (model.Attachment is not null)
            {
                var stream = model.Attachment.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024);
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.Attachment.ContentType);
                content.Add(fileContent, "Attachment", model.Attachment.Name);
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, "api/contacts");
            request.Content = content;
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(_languageState.CurrentLanguage));

            HttpResponseMessage httpResponse;

            try
            {
                httpResponse = await _httpClient.SendAsync(request);
            }
            catch
            {
                return new ApiResponseModel<Guid> { Success = false, Message = "Error de conexión." };
            }

            var json = await httpResponse.Content.ReadAsStringAsync();

            if (httpResponse.IsSuccessStatusCode)
            {
                var success = JsonSerializer.Deserialize<ApiResponseModel<Guid>>(json, JsonOptions);

                if (success is not null)
                    return success;
            }
            else
            {
                var error = JsonSerializer.Deserialize<ApiResponseModel<object>>(json, JsonOptions);

                if (error is not null)
                {
                    return new ApiResponseModel<Guid>
                    {
                        Success = false,
                        Message = error.Message,
                        Errors = error.Errors
                    };
                }
            }

            return new ApiResponseModel<Guid> { Success = false, Message = "Error inesperado." };
        }

        public async Task<ApiResponseModel<object>> UpdateAsync(UpdateContactModel model)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(model.FirstName), "FirstName");
            content.Add(new StringContent(model.LastName), "LastName");
            content.Add(new StringContent(model.Comments), "Comments");

            if (!string.IsNullOrEmpty(model.SecondName))
                content.Add(new StringContent(model.SecondName), "SecondName");

            if (!string.IsNullOrEmpty(model.SecondLastName))
                content.Add(new StringContent(model.SecondLastName), "SecondLastName");

            if (model.Attachment is not null)
            {
                var stream = model.Attachment.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024);
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.Attachment.ContentType);
                content.Add(fileContent, "Attachment", model.Attachment.Name);
            }

            using var request = new HttpRequestMessage(HttpMethod.Put, $"api/contacts/{model.Id}");
            request.Content = content;
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(_languageState.CurrentLanguage));

            HttpResponseMessage httpResponse;

            try
            {
                httpResponse = await _httpClient.SendAsync(request);
            }
            catch
            {
                return new ApiResponseModel<object> { Success = false, Message = "Error de conexión." };
            }

            var json = await httpResponse.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ApiResponseModel<object>>(json, JsonOptions);

            if (result is not null)
                return result;

            return new ApiResponseModel<object> { Success = false, Message = "Error inesperado." };
        }

        public async Task<ApiResponseModel<object>> DeleteAsync(Guid id)
        {
            using var request = new HttpRequestMessage(HttpMethod.Delete, $"api/contacts/{id}");
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(_languageState.CurrentLanguage));

            HttpResponseMessage httpResponse;

            try
            {
                httpResponse = await _httpClient.SendAsync(request);
            }
            catch
            {
                return new ApiResponseModel<object> { Success = false, Message = "Error de conexión." };
            }

            var json = await httpResponse.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ApiResponseModel<object>>(json, JsonOptions);

            if (result is not null)
                return result;

            return new ApiResponseModel<object> { Success = false, Message = "Error inesperado." };
        }
    }
}
