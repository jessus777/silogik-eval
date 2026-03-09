using System.Net;
using SilogikEval.Application.Exceptions;
using SilogikEval.Application.Interfaces;
using SilogikEval.Application.Responses;

namespace SilogikEval.Api.Middleware
{
    public class ErrorLocalizationMiddleware
    {
        private readonly RequestDelegate _next;
        private const string DefaultLanguage = "es";

        public ErrorLocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITranslationServiceAsync translationService)
        {
            try
            {
                await _next(context);
            }
            catch (AppValidationException ex)
            {
                var lang = GetLanguage(context);
                var translations = await translationService.GetTranslationsAsync(lang);

                var translatedErrors = ex.Errors.Select(e =>
                    translations.TryGetValue(e.ErrorCode, out var translated)
                        ? translated
                        : e.ErrorMessage
                ).ToList();

                var response = ApiResponse<object>.Fail(
                    translations.TryGetValue("message.error", out var msg) ? msg : ex.Message,
                    translatedErrors);

                await WriteResponseAsync(context, HttpStatusCode.BadRequest, response);
            }
            catch (BusinessException ex)
            {
                var lang = GetLanguage(context);
                var translations = await translationService.GetTranslationsAsync(lang);

                var message = translations.TryGetValue(ex.ErrorKey, out var translated)
                    ? translated
                    : ex.Message;

                var response = ApiResponse<object>.Fail(message);

                await WriteResponseAsync(context, HttpStatusCode.Conflict, response);
            }
            catch (NotFoundException ex)
            {
                var lang = GetLanguage(context);
                var translations = await translationService.GetTranslationsAsync(lang);

                var message = translations.TryGetValue(ex.ErrorKey, out var translated)
                    ? translated
                    : ex.Message;

                var response = ApiResponse<object>.Fail(message);

                await WriteResponseAsync(context, HttpStatusCode.NotFound, response);
            }
            catch (BadHttpRequestException)
            {
                var lang = GetLanguage(context);
                var translations = await translationService.GetTranslationsAsync(lang);

                var message = translations.TryGetValue("error.invalid_request", out var translated)
                    ? translated
                    : "La solicitud no es válida.";

                var response = ApiResponse<object>.Fail(message);

                await WriteResponseAsync(context, HttpStatusCode.BadRequest, response);
            }
            catch (Exception)
            {
                var lang = GetLanguage(context);
                var translations = await translationService.GetTranslationsAsync(lang);

                var message = translations.TryGetValue("error.unexpected", out var translated)
                    ? translated
                    : "Ocurrió un error inesperado.";

                var response = ApiResponse<object>.Fail(message);

                await WriteResponseAsync(context, HttpStatusCode.InternalServerError, response);
            }
        }

        private static string GetLanguage(HttpContext context)
        {
            var acceptLanguage = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(acceptLanguage))
                return DefaultLanguage;

            var primary = acceptLanguage.Split(',').FirstOrDefault()?.Trim();

            if (string.IsNullOrWhiteSpace(primary))
                return DefaultLanguage;

            var code = primary.Split(';').FirstOrDefault()?.Trim();

            return string.IsNullOrWhiteSpace(code) ? DefaultLanguage : code[..2].ToLower();
        }

        private static async Task WriteResponseAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            ApiResponse<object> response)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
