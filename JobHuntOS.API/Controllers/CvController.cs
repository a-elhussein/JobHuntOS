using JobHuntOS.Application.DTOs.Cv;
using JobHuntOS.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobHuntOS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CvController: ControllerBase
{
    private readonly ICvService _service;

    public CvController(ICvService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrent()
    {
        var cv = await _service.GetCurrentAsync();
        if (cv == null) return NotFound();
        return Ok(cv);  
    }

    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] UpsertCvDto dto)
    {
        var cv = await _service.UpsertAsync(dto);
        return Ok(cv);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        var result = await _service.DeleteAsync();
        if (!result) return NotFound();
        return Ok(result);
    }
}