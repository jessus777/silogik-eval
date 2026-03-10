using Microsoft.JSInterop;

namespace SilogikEval.Web.Client.Services
{
    public interface ISweetAlertService
    {
        Task<bool> ConfirmAsync(string title, string text, string confirmButtonText = "Confirmar", string cancelButtonText = "Cancelar");
        Task ShowLoadingAsync(string title, string text = "");
        Task SuccessAsync(string title, string text = "");
        Task ErrorAsync(string title, string text = "");
        Task CloseAsync();
    }

    public class SweetAlertService : ISweetAlertService
    {
        private readonly IJSRuntime _js;

        public SweetAlertService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<bool> ConfirmAsync(string title, string text, string confirmButtonText = "Confirmar", string cancelButtonText = "Cancelar")
        {
            return await _js.InvokeAsync<bool>("SwalInterop.confirm", title, text, confirmButtonText, cancelButtonText);
        }

        public async Task ShowLoadingAsync(string title, string text = "")
        {
            await _js.InvokeVoidAsync("SwalInterop.showLoading", title, text);
        }

        public async Task SuccessAsync(string title, string text = "")
        {
            await _js.InvokeVoidAsync("SwalInterop.success", title, text);
        }

        public async Task ErrorAsync(string title, string text = "")
        {
            await _js.InvokeVoidAsync("SwalInterop.error", title, text);
        }

        public async Task CloseAsync()
        {
            await _js.InvokeVoidAsync("SwalInterop.close");
        }
    }
}
