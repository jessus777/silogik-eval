using SilogikEval.Application.Interfaces;

namespace SilogikEval.Persistence.Services
{
    public class LocalFileStorageService
        : IFileStorageService
    {
        private readonly string _basePath;

        public LocalFileStorageService(string basePath)
        {
            _basePath = basePath;
        }

        public async Task<string> SaveAsync(Stream fileStream, string fileName)
        {
            var uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            var fullPath = Path.Combine(_basePath, uniqueName);

            Directory.CreateDirectory(_basePath);

            using var output = new FileStream(fullPath, FileMode.Create);
            await fileStream.CopyToAsync(output);

            return uniqueName;
        }

        public Task<bool> DeleteAsync(string filePath)
        {
            var fullPath = Path.Combine(_basePath, filePath);

            if (!File.Exists(fullPath))
                return Task.FromResult(false);

            File.Delete(fullPath);
            return Task.FromResult(true);
        }
    }
}
