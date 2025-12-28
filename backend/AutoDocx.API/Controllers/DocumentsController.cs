using AutoDocx.Core.DTOs;
using AutoDocx.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoDocx.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentService _documentService;

    public DocumentsController(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpPost("generate")]
    public async Task<ActionResult<DocumentResponse>> Generate([FromBody] GenerateDocumentRequest request)
    {
        try
        {
            var wordDocument = await _documentService.GenerateWordDocumentAsync(request.TemplateId, request.Data);

            // Save generated Word document temporarily
            var documentId = Guid.NewGuid();
            var wordPath = Path.Combine("temp", $"{documentId}.docx");

            Directory.CreateDirectory("temp");
            await System.IO.File.WriteAllBytesAsync(wordPath, wordDocument);

            var response = new DocumentResponse
            {
                DocumentId = documentId,
                PreviewUrl = $"/api/documents/{documentId}/preview",
                WordDownloadUrl = $"/api/documents/{documentId}/download/word",
                PdfDownloadUrl = $"/api/documents/{documentId}/download/pdf",
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{documentId}/download/{format}")]
    public async Task<IActionResult> Download(Guid documentId, string format)
    {
        var filePath = Path.Combine("temp", $"{documentId}.docx");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        var contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        var fileName = format.ToLower() == "word" ? "document.docx" : "document.docx";

        return File(fileBytes, contentType, fileName);
    }

    [HttpGet("{documentId}/preview")]
    public IActionResult Preview(Guid documentId)
    {
        // Preview not available - PDF conversion not supported on macOS/Linux
        return BadRequest(new { message = "Preview is not available on this platform. Please download the Word document instead." });
    }
}
