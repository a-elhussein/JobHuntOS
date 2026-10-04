using JobHuntOS.Application.DTOs.Analysis;
using JobHuntOS.Application.Interfaces;
using JobHuntOS.Application.Services.Interfaces;
using JobHuntOS.Domain.Entities;
using JobHuntOS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobHuntOS.Infrastructure.Repositories;

public class AnalysisRepository: IAnalysisRepository
{
    private readonly AppDbContext _context;

    public AnalysisRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<AnalysisResult?> GetByApplicationIdAsync(Guid applicationId)
    {
        return await _context.AnalysisResults
            .FirstOrDefaultAsync(x => x.ApplicationId == applicationId);
    }

    public async Task AddAsync(AnalysisResult result)
    {
        await _context.AnalysisResults.AddAsync(result);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}