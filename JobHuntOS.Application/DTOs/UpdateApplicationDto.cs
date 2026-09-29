using JobHuntOS.Domain.Enums;

namespace JobHuntOS.Application.DTOs;

public class UpdateApplicationDto
{
    public string? CompanyName { get; set; }
    public string? JobTitle { get; set; }
    public string? JobUrl { get; set; }
    public string? JobDescription { get; set; }
    public ApplicationStatus? Status { get; set; }
    public string? Location { get; set; }
    public bool? IsRemote { get; set; }
    public int? SalaryMin { get; set; }
    public int? SalaryMax { get; set; }
    public DateTime? FollowUpDate { get; set; }
}