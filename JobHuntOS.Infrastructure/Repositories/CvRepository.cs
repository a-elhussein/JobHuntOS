using JobHuntOS.Application.Interfaces;
using JobHuntOS.Domain.Entities;
using JobHuntOS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobHuntOS.Infrastructure.Repositories;

public class CvRepository: ICvRepository
{
    private readonly AppDbContext _context;

    public CvRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<UserCV?> GetCurrentAsync()
    {
        return await _context.UserCVs
            .OrderByDescending(c => c.UploadedAt)
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(UserCV cv)
    {
        await  _context.UserCVs.AddAsync(cv);
    }

    public async Task DeleteAsync(UserCV cv)
    {
         _context.UserCVs.Remove(cv);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}