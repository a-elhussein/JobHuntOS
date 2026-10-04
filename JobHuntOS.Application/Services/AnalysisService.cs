using JobHuntOS.Application.DTOs.Analysis;
using JobHuntOS.Application.Interfaces;
using JobHuntOS.Application.Services.Interfaces;
using JobHuntOS.Domain.Entities;

namespace JobHuntOS.Application.Services;

public class AnalysisService : IAnalysisService
{
    private readonly IClaudeApiService _claudeApiService;
    private readonly ICvRepository _cvRepository;
    private readonly IAnalysisRepository _analysisRepository;

    public AnalysisService(IClaudeApiService claudeApiService, ICvRepository cvRepository,
        IAnalysisRepository analysisRepository)
    {
        _claudeApiService = claudeApiService;
        _cvRepository = cvRepository;
        _analysisRepository = analysisRepository;
    }

    public async Task<AnalysisResponseDto> AnalyseAsync(AnalysisRequestDto dto)
    {
        var cv = await _cvRepository.GetCurrentAsync();
        if (cv == null)
            throw new KeyNotFoundException("Cv not found");

        var result = await _claudeApiService.AnalyseAsync(cv.Content, dto.JobDescription);

        var analysisResult = new AnalysisResult
        {
            Id = Guid.NewGuid(),
            ApplicationId = dto.ApplicationId,
            FitScore = result.FitScore,
            MatchingSkills = result.MatchingSkills,
            MissingSkills = result.MissingSkills,
            Recommendations = result.Recommendations,
            RawResponse = string.Empty,
            AnalysedAt = DateTime.UtcNow
        };

        await _analysisRepository.AddAsync(analysisResult);
        await _analysisRepository.SaveChangesAsync();
        return MapToResponseDto(analysisResult);
    }

    public async Task<AnalysisResponseDto?> GetByApplicationIdAsync(Guid applicationId)
    {
        var result = await _analysisRepository.GetByApplicationIdAsync(applicationId);
        return result == null ? null : MapToResponseDto(result);
    }


    private static AnalysisResponseDto MapToResponseDto(AnalysisResult result)
    {
        return new AnalysisResponseDto
        {
            Id = result.Id,
            ApplicationId = result.ApplicationId,
            FitScore = result.FitScore,
            MatchingSkills = result.MatchingSkills,
            MissingSkills = result.MissingSkills,
            Recommendations = result.Recommendations,
            AnalysedAt = result.AnalysedAt
        };
    }
}