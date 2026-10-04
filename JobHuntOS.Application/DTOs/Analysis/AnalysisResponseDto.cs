namespace JobHuntOS.Application.DTOs.Analysis;

public class AnalysisResponseDto
{
    public Guid Id { get; set; }
    public Guid? ApplicationId { get; set; }
    public int FitScore { get; set; }
    public List<string> MatchingSkills { get; set; } = new();
    public List<string> MissingSkills { get; set; } = new();
    public string? Recommendations { get; set; }
    public DateTime AnalysedAt { get; set; }
}