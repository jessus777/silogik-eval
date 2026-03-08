namespace SilogikEval.Application.Interfaces
{
    public interface IFileValidator
    {
        void Validate(string fileName, string contentType, long size);

    }
}
