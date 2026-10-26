using JobHuntOS.Application.DTOs.Application;
using JobHuntOS.Application.Interfaces;
using JobHuntOS.Application.Services.Interfaces;
using JobHuntOS.Domain.Entities;
using JobHuntOS.Domain.Enums;

namespace JobHuntOS.Application.Services;

public class ApplicationService: IApplicationService
{
    private readonly IApplicationRepository _repository;
    private readonly ICurrentUserService _currentUser;

    public ApplicationService(IApplicationRepository repository, ICurrentUserService  currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }
    
    
    public async Task<IEnumerable<ApplicationResponseDto>> GetAllAsync()
    {
        var applications = await _repository.GetAllAsync(_currentUser.UserId);
        return applications.Select(MapToResponseDto).ToList();
    }

    public async Task<IEnumerable<ApplicationResponseDto>> GetByStatusAsync(ApplicationStatus status)
    {
        var applications = await _repository.GetByStatusAsync(status, _currentUser.UserId);
        return applications.Select(MapToResponseDto).ToList();
    }

    public async Task<ApplicationResponseDto?> GetByIdAsync(Guid id)
    {
        var application = await _repository.GetByIdAsync(id, _currentUser.UserId);
        return application == null ? null : MapToResponseDto(application);
    }

    public async Task<ApplicationResponseDto> CreateAsync(CreateApplicationDto dto)
    {
        var application = new JobApplication
        {
            Id = Guid.NewGuid(),
            UserId = _currentUser.UserId,
            CompanyName = dto.CompanyName,
            JobTitle = dto.JobTitle,
            JobUrl = dto.JobUrl,
            JobDescription = dto.JobDescription,
            Location = dto.Location,
            IsRemote = dto.IsRemote,
            SalaryMin = dto.SalaryMin,
            SalaryMax = dto.SalaryMax,
            AppliedDate = dto.AppliedDate,
            Status = ApplicationStatus.Applied,
            FollowUpDate = dto.AppliedDate.AddDays(dto.FollowUpDays),
            CreatedAt = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow
        };
        await _repository.AddAsync(application);
        await _repository.SaveChangesAsync();
        
        return MapToResponseDto(application);
    }

    public async Task<ApplicationResponseDto?> UpdateAsync(Guid id, UpdateApplicationDto dto)
    {
        var application = await _repository.GetByIdAsync(id, _currentUser.UserId);
        if (application == null) return null;
        
        if (dto.CompanyName != null) application.CompanyName = dto.CompanyName;
        if (dto.JobTitle != null) application.JobTitle = dto.JobTitle;
        if (dto.JobUrl != null) application.JobUrl = dto.JobUrl;
        if (dto.JobDescription != null) application.JobDescription = dto.JobDescription;
        if (dto.Status.HasValue) application.Status = dto.Status.Value;
        if (dto.Location != null) application.Location = dto.Location;
        if (dto.IsRemote.HasValue) application.IsRemote = dto.IsRemote.Value;
        if (dto.SalaryMin.HasValue) application.SalaryMin = dto.SalaryMin.Value;
        if (dto.SalaryMax.HasValue) application.SalaryMax = dto.SalaryMax.Value;
        if (dto.FollowUpDate.HasValue) application.FollowUpDate = dto.FollowUpDate.Value;
        
        await _repository.UpdateAsync(application);
        await _repository.SaveChangesAsync();
        
        return MapToResponseDto(application);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var application = await _repository.GetByIdAsync(id, _currentUser.UserId);
        if (application == null) return false;
        
        await _repository.DeleteAsync(application);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ApplicationResponseDto>> GetOverdueFollowUpsAsync()
    {
        var application = await _repository.GetOverDueFollowUpAsync(_currentUser.UserId);
        return application.Select(MapToResponseDto).ToList();
    }

    private static ApplicationResponseDto MapToResponseDto(JobApplication application)
    {
        return new ApplicationResponseDto
        {
            Id = application.Id,
            CompanyName = application.CompanyName,
            JobTitle = application.JobTitle,
            JobUrl = application.JobUrl,
            JobDescription = application.JobDescription,
            Status = application.Status,
            Location = application.Location,
            IsRemote = application.IsRemote,
            SalaryMin = application.SalaryMin,
            SalaryMax = application.SalaryMax,
            AppliedDate = application.AppliedDate,
            FollowUpDate = application.FollowUpDate,
            CreatedAt = application.CreatedAt,
            LastUpdated = application.LastUpdated
        };
    }
}