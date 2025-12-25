namespace AutoDocx.Core.DTOs;

public class TemplateResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string WordFilePath { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<TemplateFieldResponse> Fields { get; set; } = new();
}

public class TemplateFieldResponse
{
    public Guid Id { get; set; }
    public string FieldKey { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public List<string>? Options { get; set; }
    public string Placeholder { get; set; } = string.Empty;
    public int Order { get; set; }
}
