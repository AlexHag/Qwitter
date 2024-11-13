using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Qwitter.Core.Application.Authentication;
using Qwitter.Core.Application.Exceptions;

namespace Qwitter.Core.Application;

public static class WebApplicationServiceExtensions
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
    {
        builder.Services.AddLogging(p => p.AddConsole());
        
        builder.Services.AddSingleton<ILogger>(p => 
        {
            var factory = p.GetRequiredService<ILoggerFactory>();
            return factory.CreateLogger("Qwitter");
        });

        builder.AddJwtAuthentication();
        builder.AddMtlsAuthentication();

        builder.Services.AddControllers().AddControllersAsServices();

        builder.AddSwagger();

        return builder;
    }

    public static WebApplication ConfigureApp(this WebApplication app)
    {
        app.UseMiddleware<RestApiExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));

        app.UseHttpsRedirection();
        app.UseAuthentication();

        app.UseRouting();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Qwitter API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

        return builder;
    }
}