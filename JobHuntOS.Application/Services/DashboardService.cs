using JobHuntOS.Application.DTOs.Dashboard;
using JobHuntOS.Application.Interfaces;
using JobHuntOS.Application.Services.Interfaces;
using JobHuntOS.Domain.Enums;

namespace JobHuntOS.Application.Services;

public class DashboardService: IDashboardService
{
    private readonly IApplicationRepository _repository;

    public DashboardService(IApplicationRepository repository)
    {
        _repository = repository;
    }
    public async Task<DashboardStatsDto> GetStatsAsync()
    {
        var all = await _repository.GetAllAsync();
        var applications = all.ToList();

        var overdue = (await _repository.GetOverDueFollowUpAsync()).ToList();

        var total = applications.Count;
        var interviews = applications.Count(a => a.Status == ApplicationStatus.Interview);
        var offers = applications.Count(a => a.Status == ApplicationStatus.Offer);
        var rejected = applications.Count(a => a.Status == ApplicationStatus.Rejected);

        var responseRate = total == 0 ? 0 : 
            Math.Round((double)(interviews + offers + rejected) / total * 100, 1);
        
        return new DashboardStatsDto
        {
            TotalApplications = total,
            Applied = applications.Count(a => a.Status == ApplicationStatus.Applied),
            Interviews = interviews,
            Offers = offers,
            Rejected = rejected,
            Withdrawn = applications.Count(a => a.Status == ApplicationStatus.Withdrawn),
            OverdueFollowUps = overdue.Count(),
            ResponseRate = responseRate
        };
    }
}