namespace AutoDocx.Core.DTOs;

public class CreateTemplateRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<TemplateFieldDto> Fields { get; set; } = new();
}

public class TemplateFieldDto
{
    public string FieldKey { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public List<string>? Options { get; set; }
    public string Placeholder { get; set; } = string.Empty;
    public int Order { get; set; }
}
