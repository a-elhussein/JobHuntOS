using JobHuntOS.Application.Interfaces;
using JobHuntOS.Domain.Entities;
using JobHuntOS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobHuntOS.Infrastructure.Repositories;

public class NoteRepository: INoteRepository
{
    private readonly AppDbContext _context;

    public NoteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationNote?> GetByIdAsync(Guid id)
    {
        return await _context.ApplicationNote
            .FirstOrDefaultAsync(n => n.Id == id);
        
    }

    public async Task<IEnumerable<ApplicationNote>> GetAllAsync()
    {
        return await _context.ApplicationNote.ToListAsync();
    }
    
    public async Task<IEnumerable<ApplicationNote>> GetByApplicationIdAsync(Guid applicationId)
    {
        return await _context.ApplicationNote.Where(n => n.ApplicationId == applicationId)
            .OrderByDescending(n => n.Created)
            .ToListAsync();
    }

    public async Task AddAsync(ApplicationNote entity)
    {
        await _context.ApplicationNote.AddAsync(entity);
    }

    public async Task UpdateAsync(ApplicationNote entity)
    {
         _context.ApplicationNote.Update(entity);
    }

    public async Task DeleteAsync(ApplicationNote entity)
    {
        _context.ApplicationNote.Remove(entity);
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}