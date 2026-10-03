using JobHuntOS.Application.DTOs.Cv;

namespace JobHuntOS.Application.Services.Interfaces;

public interface ICvService
{
    Task<CvResponseDto?> GetCurrentAsync();
    Task<CvResponseDto> UpsertAsync(UpsertCvDto dto);
    Task<bool> DeleteAsync();
}