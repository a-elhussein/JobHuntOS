using JobHuntOS.Application.Interfaces;
using JobHuntOS.Application.Services;
using JobHuntOS.Application.Services.Interfaces;
using JobHuntOS.Infrastructure.Data;
using JobHuntOS.Infrastructure.ExternalServices;
using JobHuntOS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JobHuntOS.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructre(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration
            .GetConnectionString("DefaultConnection")));

        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<IApplicationService, ApplicationService>();
        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<ICvRepository, CvRepository>();
        services.AddScoped<ICvService, CvService>();
        services.AddHttpClient<IClaudeApiService, ClaudeApiService>();
        services.AddScoped<IAnalysisRepository, AnalysisRepository>();
        services.AddScoped<IAnalysisService, AnalysisService>();
        
        return services;
    }
}