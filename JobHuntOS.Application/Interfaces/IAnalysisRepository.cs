using JobHuntOS.Domain.Entities;

namespace JobHuntOS.Application.Interfaces;

public interface IAnalysisRepository
{
    Task<AnalysisResult?> GetByApplicationIdAsync(Guid applicationId, Guid userId);
    Task AddAsync(AnalysisResult result);
    Task<int> SaveChangesAsync();
}