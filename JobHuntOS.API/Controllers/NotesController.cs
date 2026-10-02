using JobHuntOS.Application.DTOs.Note;
using JobHuntOS.Application.Services.Interfaces;
using JobHuntOS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JobHuntOS.API.Controllers;

[ApiController]
[Route("api/applications/{applicationId:guid}/notes")]
public class NotesController:ControllerBase
{
    private readonly INoteService _service;

    public NotesController(INoteService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetByApplicationId(Guid applicationId)
    {
        var notes = await _service.GetByApplicationIdAsync(applicationId);
        return Ok(notes);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid applicationId, [FromBody] CreateNoteDto dto)
    {
        var note = await _service.CreateAsync(applicationId, dto);
        return CreatedAtAction(nameof(GetByApplicationId), new { applicationId }, note);

    }

    [HttpDelete("{noteId:guid}")]
    public async Task<IActionResult> Delete(Guid noteId)
    {
        var result = await _service.DeleteAsync(noteId);
        if (!result) return NotFound();
        return NoContent();
    }
}