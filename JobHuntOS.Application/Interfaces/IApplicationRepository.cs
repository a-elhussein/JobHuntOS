using JobHuntOS.Domain.Entities;
using JobHuntOS.Domain.Enums;

namespace JobHuntOS.Application.Interfaces;

public interface IApplicationRepository: IRepository<JobApplication>
{
    Task<IEnumerable<JobApplication>> GetByStatusAsync(ApplicationStatus status);
    Task<IEnumerable<JobApplication>> GetOverDueFollowUpAsync();
    Task<JobApplication?> GetWithNotesAndAnalysisAsync(Guid id);
}