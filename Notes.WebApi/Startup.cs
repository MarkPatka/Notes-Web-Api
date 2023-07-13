using Notes.Application;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Persistance;
using Notes.WebApi.Middleware;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Notes.WebApi;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => 
        Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            config.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
        });

        services.AddApplication();
        services.AddPersistence(Configuration);
        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });

        services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;

            config.DefaultChallengeScheme = 
                JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer("Bearer", options => {
               options.Authority = "http://localhost:44323/";
               options.Audience = "NotesWebAPI";
               options.RequireHttpsMetadata = false;
            });

        services.AddVersionedApiExplorer(options =>
            options.GroupNameFormat = "'v'VVV");

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();



        services.AddSwaggerGen(config => {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            config.IncludeXmlComments(xmlPath);
        });

        services.AddApiVersioning();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(config => {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                config.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());

                config.RoutePrefix = string.Empty;
            }
            config.RoutePrefix = string.Empty;
            config.SwaggerEndpoint("swagger/v1/swagger.json", "Notes API");
        });

        app.UseCustomExceptionHandler();
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseApiVersioning();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
