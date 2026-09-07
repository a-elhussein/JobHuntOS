namespace JobHuntOS.Domain.Entities;

public class AnalysisResult
{
    public Guid Id { get; set; }
    public Guid? ApplicationId { get; set; }
    public int FitScore { get; set; }
    public List<string> MatchingSkills { get; set; } = new();
    public List<string> MissingSkills { get; set; } = new();
    public string? Recommendations { get; set; }
    public string? RawResponse { get; set; }
    public DateTime AnalysedAt { get; set; } = DateTime.UtcNow;
    
    public JobApplication? Application { get; set; }
}