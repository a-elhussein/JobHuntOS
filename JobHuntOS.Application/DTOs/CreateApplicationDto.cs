namespace JobHuntOS.Application.DTOs;

public class CreateApplicationDto
{
    public string CompanyName { get; set; } = String.Empty;
    public string JobTitle { get; set; } =  String.Empty;
    public string? JobUrl { get; set; }
    public string? JobDescription { get; set; }
    public string? Location { get; set; }
    public bool IsRemote { get; set; }
    public int? SalaryMin { get; set; }
    public int? SalaryMax { get; set; }
    public DateTime AppliedDate { get; set; }
    public int FollowUpDays { get; set; } = 7;
}