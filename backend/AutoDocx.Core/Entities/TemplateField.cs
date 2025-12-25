namespace AutoDocx.Core.Entities;

public class TemplateField
{
    public Guid Id { get; set; }
    public Guid TemplateId { get; set; }
    public string FieldKey { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public string? OptionsJson { get; set; }
    public string Placeholder { get; set; } = string.Empty;
    public int Order { get; set; }
    
    // Navigation properties
    public Template Template { get; set; } = null!;
}
