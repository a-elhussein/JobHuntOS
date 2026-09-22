using JobHuntOS.Application.Interfaces;
using JobHuntOS.Domain.Entities;
using JobHuntOS.Domain.Enums;
using JobHuntOS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobHuntOS.Infrastructure.Repositories;

public class ApplicationRepository: IApplicationRepository
{
    private readonly AppDbContext _context;

    public ApplicationRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<JobApplication?> GetByIdAsync(Guid id)
    {
        return await _context.JobApplications.FirstOrDefaultAsync
            (a => a.Id == id && !a.IsDeleted);
    }

    public async Task<IEnumerable<JobApplication>> GetAllAsync()
    {
        return await _context.JobApplications
            .Where(a => !a.IsDeleted)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(); 
    }

    public async Task AddAsync(JobApplication entity)
    {
         await _context.JobApplications.AddAsync(entity);
    }

    public async Task UpdateAsync(JobApplication entity)
    {
        entity.LastUpdated = DateTime.UtcNow;
        _context.JobApplications.Update(entity);
    }

    public async Task DeleteAsync(JobApplication entity)
    {
        entity.IsDeleted = true;
        entity.LastUpdated = DateTime.UtcNow;
        _context.JobApplications.Update(entity);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetByStatusAsync(ApplicationStatus status)
    {
        return await _context.JobApplications
            .Where(a => a.Status == status && !a.IsDeleted)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetOverDueFollowUpAsync()
    {
        var activeStatuses = new[] { ApplicationStatus.Applied, ApplicationStatus.Interview };
        
        return await _context.JobApplications
            .Where(a => !a.IsDeleted
            && a.FollowUpDate.HasValue
            && a.FollowUpDate.Value.Date < DateTime.UtcNow
            && activeStatuses.Contains(a.Status))
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<JobApplication?> GetWithNotesAndAnalysisAsync(Guid id)
    {
        return await _context.JobApplications
            .Include(a => a.Notes)
            .Include(a => a.AnalysisResult)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
    }
}