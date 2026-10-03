using JobHuntOS.Domain.Entities;

namespace JobHuntOS.Application.Interfaces;

public interface ICvRepository
{
    Task<UserCV?> GetCurrentAsync();
    Task AddAsync(UserCV cv);
    Task DeleteAsync(UserCV cv);
    Task<int> SaveChangesAsync();
}