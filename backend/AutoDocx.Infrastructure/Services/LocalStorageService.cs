using AutoDocx.Core.Interfaces;

namespace AutoDocx.Infrastructure.Services;

public class LocalStorageService : IStorageService
{
    private readonly string _storagePath;

    public LocalStorageService(string storagePath = "Storage")
    {
        _storagePath = storagePath;
        Directory.CreateDirectory(_storagePath);
    }

    public async Task<string> SaveFileAsync(string fileName, Stream fileStream)
    {
        var filePath = Path.Combine(_storagePath, fileName);
        
        using var fileStreamOutput = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await fileStream.CopyToAsync(fileStreamOutput);
        
        return filePath;
    }

    public async Task<byte[]> GetFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        return await File.ReadAllBytesAsync(filePath);
    }

    public Task DeleteFileAsync(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        return Task.CompletedTask;
    }
}
