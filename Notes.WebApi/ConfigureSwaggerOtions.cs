using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Notes.WebApi;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            var apiVersion = description.ApiVersion.ToString();
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Version = apiVersion,
                Title = $"Notes API {apiVersion}",
                Description = "This sample was created by this tutorial (link below)",                
                TermsOfService = new Uri("https://www.youtube.com/playlist?list=PLEtg-LdqEKXbpq4RtUp1hxZ6ByGjnvQs4"),
                Contact = new OpenApiContact
                {
                    Name = "Support",
                    Email = string.Empty,
                    Url = new Uri("https://t.me/The_Holy_Other")
                },
                License = new OpenApiLicense
                {
                    Name = "Contact me",
                    Url = new Uri("https://t.me/The_Holy_Other")
                }
            });

            options.AddSecurityDefinition($"AuthToken {apiVersion}",
                new OpenApiSecurityScheme 
                { 
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Name = "Authorization",
                    Description = "Authorization token"
                });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = $"AuthToken {apiVersion}"
                        }
                    },
                    new string[] {}
                }
            });

            options.CustomOperationIds(apiDescription => 
            apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null);
        }
    }
    
}