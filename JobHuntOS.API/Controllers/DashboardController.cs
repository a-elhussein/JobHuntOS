using System.Threading.Tasks;
using JobHuntOS.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobHuntOS.API.Controllers;

[ApiController]
[Route("api/dashboard")]

public class DashboardController: ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var stats = await _dashboardService.GetStatsAsync();
        return Ok(stats);
    }
}