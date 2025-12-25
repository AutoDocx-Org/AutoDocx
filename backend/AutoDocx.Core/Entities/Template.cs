namespace AutoDocx.Core.Entities;

public class Template
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string WordFilePath { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid? UserId { get; set; }
    
    // Navigation properties
    public ICollection<TemplateField> Fields { get; set; } = new List<TemplateField>();
}
