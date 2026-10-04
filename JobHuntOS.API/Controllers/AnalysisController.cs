using JobHuntOS.Application.DTOs.Analysis;
using JobHuntOS.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobHuntOS.API.Controllers;

[ApiController]
[Route("api/analysis")]
public class AnalysisController:ControllerBase
{
    private readonly IAnalysisService _analysisService;

    public AnalysisController(IAnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    [HttpPost]
    public async Task<IActionResult> Analyse([FromBody] AnalysisRequestDto dto)
    {
        var result = await _analysisService.AnalyseAsync(dto);
        return Ok(result);
    }

    [HttpGet("{applicationId:guid}")]
    public async Task<IActionResult> GetApplicationId(Guid applicationId)
    {
        var result = await _analysisService.GetByApplicationIdAsync(applicationId);
        if (result == null) return NotFound();
        return Ok(result);
    }
}