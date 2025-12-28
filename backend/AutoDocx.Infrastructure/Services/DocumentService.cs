using AutoDocx.Core.Interfaces;
using AutoDocx.Infrastructure.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;

namespace AutoDocx.Infrastructure.Services;

public class DocumentService : IDocumentService
{
    private readonly AutoDocxDbContext _context;
    private readonly IStorageService _storageService;

    public DocumentService(AutoDocxDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    public async Task<byte[]> GenerateWordDocumentAsync(Guid templateId, Dictionary<string, object> data)
    {
        // Fetch template and fields
        var template = await _context.Templates
            .Include(t => t.Fields)
            .FirstOrDefaultAsync(t => t.Id == templateId);

        if (template == null)
        {
            throw new InvalidOperationException("Template not found");
        }

        // Validate required fields
        var requiredFields = template.Fields.Where(f => f.IsRequired).ToList();
        foreach (var field in requiredFields)
        {
            if (!data.ContainsKey(field.FieldKey) || 
                string.IsNullOrEmpty(data[field.FieldKey]?.ToString()))
            {
                throw new InvalidOperationException($"Required field '{field.Label}' is missing");
            }
        }

        // Load template file
        byte[] templateBytes = await _storageService.GetFileAsync(template.WordFilePath);

        using var memoryStream = new MemoryStream();
        memoryStream.Write(templateBytes, 0, templateBytes.Length);

        using (WordprocessingDocument doc = WordprocessingDocument.Open(memoryStream, true))
        {
            // Replace placeholders
            foreach (var text in doc.MainDocumentPart!.Document.Descendants<Text>())
            {
                foreach (var field in template.Fields)
                {
                    if (text.Text.Contains(field.Placeholder))
                    {
                        var value = data.ContainsKey(field.FieldKey)
                            ? data[field.FieldKey]?.ToString() ?? ""
                            : "";

                        text.Text = text.Text.Replace(field.Placeholder, value);
                    }
                }
            }

            doc.Save();
        }

        return memoryStream.ToArray();
    }

    public async Task<byte[]> ConvertToPdfAsync(byte[] wordDocument)
    {
        await Task.CompletedTask;
        
        // FreeSpire.Doc uses System.Drawing.Common which doesn't work on macOS/Linux
        // For non-Windows platforms, we'll need a different solution
        if (!OperatingSystem.IsWindows())
        {
            // Return the Word document as-is for now on non-Windows platforms
            // In production, consider using:
            // - LibreOffice in headless mode
            // - Cloud-based conversion service (e.g., CloudConvert API)
            // - Commercial library like Aspose.Words that supports cross-platform
            return wordDocument;
        }
        
        // Save the Word document to a temporary file
        var tempWordPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.docx");
        var tempPdfPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");

        try
        {
            // Write Word document to temp file
            await File.WriteAllBytesAsync(tempWordPath, wordDocument);

            // Use Free Spire.Doc to convert (Windows only)
            var document = new Spire.Doc.Document();
            document.LoadFromFile(tempWordPath);
            document.SaveToFile(tempPdfPath, Spire.Doc.FileFormat.PDF);

            // Read the PDF file
            var pdfBytes = await File.ReadAllBytesAsync(tempPdfPath);
            return pdfBytes;
        }
        finally
        {
            // Clean up temporary files
            if (File.Exists(tempWordPath))
                File.Delete(tempWordPath);
            if (File.Exists(tempPdfPath))
                File.Delete(tempPdfPath);
        }
    }
}
