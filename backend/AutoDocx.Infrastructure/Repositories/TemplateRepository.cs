using AutoDocx.Core.Entities;
using AutoDocx.Core.Interfaces;
using AutoDocx.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoDocx.Infrastructure.Repositories;

public class TemplateRepository : ITemplateRepository
{
    private readonly AutoDocxDbContext _context;

    public TemplateRepository(AutoDocxDbContext context)
    {
        _context = context;
    }

    public async Task<Template?> GetByIdAsync(Guid id)
    {
        return await _context.Templates
            .Include(t => t.Fields.OrderBy(f => f.Order))
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Template>> GetAllAsync()
    {
        return await _context.Templates
            .Include(t => t.Fields)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<Template> CreateAsync(Template template)
    {
        template.Id = Guid.NewGuid();
        template.CreatedAt = DateTime.UtcNow;
        template.UpdatedAt = DateTime.UtcNow;

        await _context.Templates.AddAsync(template);
        await _context.SaveChangesAsync();
        return template;
    }

    public async Task<Template> UpdateAsync(Template template)
    {
        template.UpdatedAt = DateTime.UtcNow;
        _context.Templates.Update(template);
        await _context.SaveChangesAsync();
        return template;
    }

    public async Task DeleteAsync(Guid id)
    {
        var template = await _context.Templates.FindAsync(id);
        if (template != null)
        {
            _context.Templates.Remove(template);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Templates.AnyAsync(t => t.Id == id);
    }
}
