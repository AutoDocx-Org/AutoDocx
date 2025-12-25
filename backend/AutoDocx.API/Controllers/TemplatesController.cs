using AutoDocx.Core.DTOs;
using AutoDocx.Core.Entities;
using AutoDocx.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AutoDocx.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplatesController : ControllerBase
{
    private readonly ITemplateRepository _templateRepository;
    private readonly IStorageService _storageService;

    public TemplatesController(ITemplateRepository templateRepository, IStorageService storageService)
    {
        _templateRepository = templateRepository;
        _storageService = storageService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TemplateResponse>>> GetAll()
    {
        var templates = await _templateRepository.GetAllAsync();
        var response = templates.Select(MapToResponse);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TemplateResponse>> GetById(Guid id)
    {
        var template = await _templateRepository.GetByIdAsync(id);
        if (template == null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(template));
    }

    [HttpPost]
    public async Task<ActionResult<TemplateResponse>> Create([FromForm] CreateTemplateRequest request, IFormFile wordFile)
    {
        if (wordFile == null || wordFile.Length == 0)
        {
            return BadRequest("Word file is required");
        }

        // Validate file type
        if (!wordFile.ContentType.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
        {
            return BadRequest("Only .docx files are allowed");
        }

        // Save file
        var fileName = $"{Guid.NewGuid()}.docx";
        var filePath = await _storageService.SaveFileAsync(fileName, wordFile.OpenReadStream());

        // Create template
        var template = new Template
        {
            Name = request.Name,
            Description = request.Description,
            WordFilePath = filePath,
            Fields = request.Fields.Select(f => new TemplateField
            {
                FieldKey = f.FieldKey,
                Label = f.Label,
                Type = f.Type,
                IsRequired = f.IsRequired,
                Placeholder = f.Placeholder,
                Order = f.Order,
                OptionsJson = f.Options != null ? JsonSerializer.Serialize(f.Options) : null
            }).ToList()
        };

        var createdTemplate = await _templateRepository.CreateAsync(template);
        return CreatedAtAction(nameof(GetById), new { id = createdTemplate.Id }, MapToResponse(createdTemplate));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TemplateResponse>> Update(Guid id, [FromForm] CreateTemplateRequest request, IFormFile? wordFile)
    {
        var template = await _templateRepository.GetByIdAsync(id);
        if (template == null)
        {
            return NotFound();
        }

        // Update word file if provided
        if (wordFile != null && wordFile.Length > 0)
        {
            // Delete old file
            await _storageService.DeleteFileAsync(template.WordFilePath);

            // Save new file
            var fileName = $"{Guid.NewGuid()}.docx";
            template.WordFilePath = await _storageService.SaveFileAsync(fileName, wordFile.OpenReadStream());
        }

        // Update template
        template.Name = request.Name;
        template.Description = request.Description;

        // Update fields
        template.Fields.Clear();
        template.Fields = request.Fields.Select(f => new TemplateField
        {
            FieldKey = f.FieldKey,
            Label = f.Label,
            Type = f.Type,
            IsRequired = f.IsRequired,
            Placeholder = f.Placeholder,
            Order = f.Order,
            OptionsJson = f.Options != null ? JsonSerializer.Serialize(f.Options) : null,
            TemplateId = id
        }).ToList();

        var updatedTemplate = await _templateRepository.UpdateAsync(template);
        return Ok(MapToResponse(updatedTemplate));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var template = await _templateRepository.GetByIdAsync(id);
        if (template == null)
        {
            return NotFound();
        }

        // Delete file
        await _storageService.DeleteFileAsync(template.WordFilePath);

        // Delete template
        await _templateRepository.DeleteAsync(id);
        return NoContent();
    }

    private static TemplateResponse MapToResponse(Template template)
    {
        return new TemplateResponse
        {
            Id = template.Id,
            Name = template.Name,
            Description = template.Description,
            WordFilePath = template.WordFilePath,
            CreatedAt = template.CreatedAt,
            UpdatedAt = template.UpdatedAt,
            Fields = template.Fields.Select(f => new TemplateFieldResponse
            {
                Id = f.Id,
                FieldKey = f.FieldKey,
                Label = f.Label,
                Type = f.Type,
                IsRequired = f.IsRequired,
                Placeholder = f.Placeholder,
                Order = f.Order,
                Options = f.OptionsJson != null ? JsonSerializer.Deserialize<List<string>>(f.OptionsJson) : null
            }).OrderBy(f => f.Order).ToList()
        };
    }
}
