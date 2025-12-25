namespace AutoDocx.Core.Interfaces;

public interface IStorageService
{
    Task<string> SaveFileAsync(string fileName, Stream fileStream);
    Task<byte[]> GetFileAsync(string filePath);
    Task DeleteFileAsync(string filePath);
}
