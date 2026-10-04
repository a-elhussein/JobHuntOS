using JobHuntOS.Application.DTOs.Analysis;

namespace JobHuntOS.Application.Services.Interfaces;

public interface IAnalysisService
{
    Task<AnalysisResponseDto> AnalyseAsync(AnalysisRequestDto dto);
    Task<AnalysisResponseDto?> GetByApplicationIdAsync(Guid applicationId);
}