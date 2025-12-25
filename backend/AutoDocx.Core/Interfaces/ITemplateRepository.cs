using AutoDocx.Core.Entities;

namespace AutoDocx.Core.Interfaces;

public interface ITemplateRepository
{
    Task<Template?> GetByIdAsync(Guid id);
    Task<IEnumerable<Template>> GetAllAsync();
    Task<Template> CreateAsync(Template template);
    Task<Template> UpdateAsync(Template template);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
