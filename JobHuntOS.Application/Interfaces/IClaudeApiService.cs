using JobHuntOS.Application.DTOs.Analysis;
using JobHuntOS.Domain.Entities;

namespace JobHuntOS.Application.Interfaces;

public interface IClaudeApiService
{
    Task<ClaudeAnalysisResult> AnalyseAsync(string cvContent, string jobDescription);
}