using JobHuntOS.Application.DTOs.Analysis;
using JobHuntOS.Application.Exceptions;
using JobHuntOS.Application.Interfaces;
using JobHuntOS.Application.Services.Interfaces;
using JobHuntOS.Domain.Entities;

namespace JobHuntOS.Application.Services;

public class AnalysisService : IAnalysisService
{
    public const int DailyLimit = 3;
    
    private readonly IClaudeApiService _claudeApiService;
    private readonly ICvRepository _cvRepository;
    private readonly IAnalysisRepository _analysisRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationRepository _applicationRepository;

    public AnalysisService(IClaudeApiService claudeApiService, ICvRepository cvRepository,
        IAnalysisRepository analysisRepository, ICurrentUserService  currentUserService, IApplicationRepository applicationRepository)
    {
        _claudeApiService = claudeApiService;
        _cvRepository = cvRepository;
        _analysisRepository = analysisRepository;
        _currentUserService = currentUserService;
        _applicationRepository = applicationRepository;
    }

    public async Task<AnalysisResponseDto> AnalyseAsync(AnalysisRequestDto dto)
    {
        var userId = _currentUserService.UserId;
        
        var usedToday = await _analysisRepository.CountSinceAsync(userId, DateTime.UtcNow.Date);
        if (usedToday >= DailyLimit)
            throw new RateLimitExceededException(
                $"Daily limit of {DailyLimit} analyses reached. Try again tomorrow.");
        
        if (dto.ApplicationId.HasValue)
        {
            var app = await _applicationRepository.GetByIdAsync(dto.ApplicationId.Value, userId);
            if (app == null)
                throw new KeyNotFoundException("Application not found");
        }
        
        var cv = await _cvRepository.GetCurrentAsync(userId);
        if (cv == null)
            throw new KeyNotFoundException("Cv not found");

        var result = await _claudeApiService.AnalyseAsync(cv.Content, dto.JobDescription);

        var analysisResult = new AnalysisResult
        {
            Id = Guid.NewGuid(),
            UserId = userId,  
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
        var result = await _analysisRepository.GetByApplicationIdAsync(applicationId, _currentUserService.UserId);
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