namespace SilogikEval.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveAsync(Stream fileStream, string fileName);

        Task<bool> DeleteAsync(string filePath);
    }
}
