using JobHuntOS.Application.DTOs;
using JobHuntOS.Domain.Enums;

namespace JobHuntOS.Application.Services.Interfaces;

public interface IApplicationService
{
    Task<IEnumerable<ApplicationResponseDto>> GetAllAsync();
    Task<IEnumerable<ApplicationResponseDto>> GetByStatusAsync(ApplicationStatus status);
    Task<ApplicationResponseDto?> GetByIdAsync(Guid id);
    Task<ApplicationResponseDto> CreateAsync(CreateApplicationDto dto);
    Task<ApplicationResponseDto> UpdateAsync(Guid id, UpdateApplicationDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<ApplicationResponseDto>> GetOverdueFollowUpsAsync();
}