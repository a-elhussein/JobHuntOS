using JobHuntOS.Application.DTOs.Note;
using JobHuntOS.Application.Interfaces;
using JobHuntOS.Application.Services.Interfaces;
using JobHuntOS.Domain.Entities;

namespace JobHuntOS.Application.Services;

public class NoteService: INoteService
{
    private readonly INoteRepository _noteRepository;
    private readonly IApplicationRepository _applicationRepository;

    public NoteService(INoteRepository noteRepository, IApplicationRepository applicationRepository)
    {
        _noteRepository = noteRepository;
        _applicationRepository = applicationRepository;
    }
    public async Task<IEnumerable<NoteResponseDto>> GetByApplicationIdAsync(Guid applicationId)
    {
        var notes = await _noteRepository.GetByApplicationIdAsync(applicationId);
        return notes.Select(MapToResponseDto);
    }

    public async Task<NoteResponseDto> CreateAsync(Guid applicationId, CreateNoteDto dto)
    {
        var application = await _applicationRepository.GetByIdAsync(applicationId);
        if (application == null)
            throw new ApplicationException($"Application with ID {applicationId} not found");

        var note = new ApplicationNote
        {
            Id = Guid.NewGuid(),
            ApplicationId = applicationId,
            Content = dto.Content,
            Created = DateTime.UtcNow

        };
        
        await _noteRepository.AddAsync(note);
        await _applicationRepository.SaveChangesAsync();
        return MapToResponseDto(note);
    }

    public async Task<bool> DeleteAsync(Guid noteId)
    {
        var note = await _noteRepository.GetByIdAsync(noteId);
        if (note == null) return false;
        
        await _noteRepository.DeleteAsync(note);
        await _applicationRepository.SaveChangesAsync();
        return true;
    }
    
    private static NoteResponseDto MapToResponseDto(ApplicationNote note)
    {
        return new NoteResponseDto
        {
            Id = note.Id,
            ApplicationId = note.ApplicationId,
            Content = note.Content,
            CreatedAt = note.Created
        };
    }
}