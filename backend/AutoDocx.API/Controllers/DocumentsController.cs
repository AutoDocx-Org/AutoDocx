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
            var pdfDocument = await _documentService.ConvertToPdfAsync(wordDocument);

            // Save generated documents temporarily
            var documentId = Guid.NewGuid();
            var wordPath = Path.Combine("temp", $"{documentId}.docx");
            var pdfPath = Path.Combine("temp", $"{documentId}.pdf");

            Directory.CreateDirectory("temp");
            await System.IO.File.WriteAllBytesAsync(wordPath, wordDocument);
            await System.IO.File.WriteAllBytesAsync(pdfPath, pdfDocument);

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
        var extension = format.ToLower() == "word" ? "docx" : "pdf";
        var filePath = Path.Combine("temp", $"{documentId}.{extension}");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        var contentType = format.ToLower() == "word"
            ? "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            : "application/pdf";

        return File(fileBytes, contentType, $"document.{extension}");
    }

    [HttpGet("{documentId}/preview")]
    public async Task<IActionResult> Preview(Guid documentId)
    {
        var pdfPath = Path.Combine("temp", $"{documentId}.pdf");

        if (!System.IO.File.Exists(pdfPath))
        {
            return NotFound();
        }

        var fileBytes = await System.IO.File.ReadAllBytesAsync(pdfPath);
        return File(fileBytes, "application/pdf");
    }
}
