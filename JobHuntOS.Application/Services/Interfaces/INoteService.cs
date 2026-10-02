using JobHuntOS.Application.DTOs.Note;

namespace JobHuntOS.Application.Services.Interfaces;

public interface INoteService
{
    Task<IEnumerable<NoteResponseDto>> GetByApplicationIdAsync(Guid applicationId);
    Task<NoteResponseDto> CreateAsync(Guid applicationId, CreateNoteDto dto);
    Task<bool> DeleteAsync(Guid noteId);
}