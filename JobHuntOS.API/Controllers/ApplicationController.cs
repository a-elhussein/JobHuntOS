using JobHuntOS.Application.DTOs.Application;
using JobHuntOS.Application.Services.Interfaces;
using JobHuntOS.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace JobHuntOS.API.Controllers;


[ApiController]
[Route("[controller]")]
public class ApplicationController: ControllerBase
{
    private readonly IApplicationService _service;

    public ApplicationController(IApplicationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ApplicationStatus? status)
    {
        if (status.HasValue)
        {
            var filteredStatus = _service.GetByStatusAsync(status.Value);
            return  Ok(filteredStatus);
        }
        
        var applications = await _service.GetAllAsync();
        return Ok(applications);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var application = await _service.GetByIdAsync(id);
        if (application == null) return NotFound();
        return Ok(application);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateApplicationDto dto){
        var application = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = application.Id }, application);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateApplicationDto dto){
        var application = await _service.UpdateAsync(id, dto);
        if (application == null) return NotFound();
        return Ok(application);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var application = await _service.DeleteAsync(id);
        if (!application) return NotFound();
        return Ok(application);
    }

    [HttpGet("reminders")]
    public async Task<IActionResult> GetOverdueFollowUps()
    {
        var applications = await _service.GetOverdueFollowUpsAsync();
        return Ok(applications);
    }
    
}