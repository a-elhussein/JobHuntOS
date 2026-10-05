using JobHuntOS.Application.DTOs.Dashboard;

namespace JobHuntOS.Application.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardStatsDto> GetStatsAsync();
}