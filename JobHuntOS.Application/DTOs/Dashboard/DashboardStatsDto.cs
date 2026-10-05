namespace JobHuntOS.Application.DTOs.Dashboard;

public class DashboardStatsDto
{
    public int TotalApplications { get; set; }
    public int Applied { get; set; }
    public int Interviews { get; set; }
    public int Offers { get; set; }
    public int Rejected { get; set; }
    public int Withdrawn { get; set; }
    public int OverdueFollowUps { get; set; }
    public double ResponseRate { get; set; }
}