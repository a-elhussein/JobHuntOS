using JobHuntOS.Domain.Enums;

namespace JobHuntOS.Domain.Entities;

public class JobApplication
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string? JobUrl { get; set; }
    public string? JobDescription { get; set; }
    public ApplicationStatus Status { get; set; }
    public string? Location { get; set; }
    public bool IsRemote { get; set; }
    public int? SalaryMin { get; set; }
    public int? SalaryMax { get; set; }
    public DateTime AppliedDate { get; set; }
    public DateTime? FollowUpDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdated { get; set; }
    
    public ICollection<ApplicationNote>  Notes { get; set; } = new List<ApplicationNote>();
    public AnalysisResult? AnalysisResult { get; set; }
}