using JobHuntOS.Domain.Entities;
using JobHuntOS.Domain.Enums;

namespace JobHuntOS.Application.Interfaces;

public interface IApplicationRepository
{
    Task<JobApplication?> GetByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<JobApplication>> GetAllAsync(Guid userId);
    Task<IEnumerable<JobApplication>> GetByStatusAsync(ApplicationStatus status, Guid userId);
    Task<IEnumerable<JobApplication>> GetOverDueFollowUpAsync(Guid userId);
    Task<JobApplication?> GetWithNotesAndAnalysisAsync(Guid id, Guid userId);
    Task AddAsync(JobApplication entity);
    Task UpdateAsync(JobApplication entity);
    Task DeleteAsync(JobApplication entity);
    Task<int> SaveChangesAsync();
}