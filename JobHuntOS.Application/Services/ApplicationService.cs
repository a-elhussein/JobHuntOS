using JobHuntOS.Application.DTOs;
using JobHuntOS.Application.Interfaces;
using JobHuntOS.Application.Services.Interfaces;
using JobHuntOS.Domain.Entities;
using JobHuntOS.Domain.Enums;

namespace JobHuntOS.Application.Services;

public class ApplicationService: IApplicationService
{
    private readonly IApplicationRepository _repository;

    public ApplicationService(IApplicationRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task<IEnumerable<ApplicationResponseDto>> GetAllAsync()
    {
        var applications = await _repository.GetAllAsync();
        return applications.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<ApplicationResponseDto>> GetByStatusAsync(ApplicationStatus status)
    {
        var applications = await _repository.GetByStatusAsync(status);
        return applications.Select(MapToResponseDto);
    }

    public async Task<ApplicationResponseDto?> GetByIdAsync(Guid id)
    {
        var application = await _repository.GetByIdAsync(id);
        return application == null ? null : MapToResponseDto(application);
    }

    public async Task<ApplicationResponseDto> CreateAsync(CreateApplicationDto dto)
    {
        var application = new JobApplication
        {
            Id = Guid.NewGuid(),
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

    public async Task<ApplicationResponseDto> UpdateAsync(Guid id, UpdateApplicationDto dto)
    {
        var application = await _repository.GetByIdAsync(id);
        if (application == null) return null;
        
        if (dto.CompanyName != null) application.CompanyName = dto.CompanyName;
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
        var application = await _repository.GetByIdAsync(id);
        if (application == null) return false;
        
        await _repository.DeleteAsync(application);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ApplicationResponseDto>> GetOverdueFollowUpsAsync()
    {
        var application = await _repository.GetOverDueFollowUpAsync();
        return application.Select(MapToResponseDto);
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