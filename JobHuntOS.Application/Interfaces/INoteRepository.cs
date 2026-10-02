using JobHuntOS.Domain.Entities;

namespace JobHuntOS.Application.Interfaces;

public interface INoteRepository: IRepository<ApplicationNote>
{
    Task<IEnumerable<ApplicationNote>> GetByApplicationIdAsync(Guid applicationId);
}