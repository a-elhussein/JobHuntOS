namespace JobHuntOS.Application.DTOs.Analysis;

public class AnalysisRequestDto
{
    public string JobDescription { get; set; } = string.Empty;
    public Guid? ApplicationId { get; set; }
}