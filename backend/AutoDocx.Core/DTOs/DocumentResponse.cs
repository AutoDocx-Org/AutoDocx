namespace AutoDocx.Core.DTOs;

public class DocumentResponse
{
    public Guid DocumentId { get; set; }
    public string PreviewUrl { get; set; } = string.Empty;
    public string WordDownloadUrl { get; set; } = string.Empty;
    public string PdfDownloadUrl { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
