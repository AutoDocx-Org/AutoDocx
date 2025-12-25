namespace AutoDocx.Core.DTOs;

public class GenerateDocumentRequest
{
    public Guid TemplateId { get; set; }
    public Dictionary<string, object> Data { get; set; } = new();
}
