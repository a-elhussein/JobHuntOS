namespace JobHuntOS.Application.DTOs.Analysis;

public class ClaudeAnalysisResult
{
    public int FitScore { get; set; }
    public List<string> MatchingSkills { get; set; } = new();
    public List<string> MissingSkills { get; set; } = new();
    public string Recommendations { get; set; } = string.Empty;
}