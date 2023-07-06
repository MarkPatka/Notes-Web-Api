
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notes.Application.Interfaces;
using System.Reflection;

namespace Notes.Persistance;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["DbConnection"];

        services.AddDbContext<NotesDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        services.AddScoped<INotesDbContext>(provider => 
            provider.GetService<NotesDbContext>() ?? throw new ArgumentNullException(nameof(NotesDbContext)));

        return services;
    }
}
