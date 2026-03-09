using SilogikEval.Application.Constants;
using SilogikEval.Application.Exceptions;
using SilogikEval.Application.Interfaces;

namespace SilogikEval.Persistence.Services
{
    public class FileValidator
        : IFileValidator
    {
        private readonly string[] _allowedExtensions =
        {
            ".jpg", ".jpeg", ".png", ".pdf"
        };

        private readonly string[] _allowedContentTypes =
        {
            "image/jpeg",
            "image/png",
            "application/pdf"
        };

        private const long MaxSize = 5 * 1024 * 1024;

        public void Validate(
            string fileName,
            string contentType,
            long size)
        {
            var extension = Path.GetExtension(fileName).ToLower();

            if (!_allowedExtensions.Contains(extension))
                throw new BusinessException(
                    ErrorKeys.FileExtensionNotAllowed,
                    "Extensión de archivo no permitida.");

            if (!_allowedContentTypes.Contains(contentType))
                throw new BusinessException(
                    ErrorKeys.FileTypeNotAllowed,
                    "Tipo de archivo no permitido.");

            if (size > MaxSize)
                throw new BusinessException(
                    ErrorKeys.FileSizeExceeded,
                    "El archivo excede el tamaño permitido.");
        }
    }
}
