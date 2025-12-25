namespace AutoDocx.Core.Interfaces;

public interface IDocumentService
{
    Task<byte[]> GenerateWordDocumentAsync(Guid templateId, Dictionary<string, object> data);
    Task<byte[]> ConvertToPdfAsync(byte[] wordDocument);
}
