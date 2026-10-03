using JobHuntOS.Application.DTOs.Cv;
using JobHuntOS.Application.Interfaces;
using JobHuntOS.Application.Services.Interfaces;
using JobHuntOS.Domain.Entities;

namespace JobHuntOS.Application.Services;

public class CvService: ICvService
{
    private readonly ICvRepository _repository;

    public CvService(ICvRepository repository)
    {
        _repository = repository;
    }
    public async Task<CvResponseDto?> GetCurrentAsync()
    {
        var cv = await _repository.GetCurrentAsync();
        return cv == null ? null : MapToResponseDto(cv);
    }

    public async Task<CvResponseDto> UpsertAsync(UpsertCvDto dto)
    {
        var existing = await _repository.GetCurrentAsync();
        if (existing != null)
        {
            await _repository.DeleteAsync(existing);
        }

        var newCv = new UserCV
        {
            Id = Guid.NewGuid(),
            Content = dto.Content,
            FileName = dto.FileName,
            UploadedAt = DateTime.UtcNow
        };
        
        await _repository.AddAsync(newCv);
        await _repository.SaveChangesAsync();
        
        return MapToResponseDto(newCv);
    }

    public async Task<bool> DeleteAsync()
    {
        var cv = await _repository.GetCurrentAsync();
        if (cv == null) return false;
        
        await _repository.DeleteAsync(cv);
        await _repository.SaveChangesAsync();
        return true;
    }
    
    private static CvResponseDto MapToResponseDto(UserCV cv)
    {
        return new CvResponseDto
        {
            Id = cv.Id,
            Content = cv.Content,
            FileName = cv.FileName,
            UploadedAt = cv.UploadedAt
        };
    }
}