using Application;
using WebUI.GlobalExeptionHandler;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.TelegramBot;
using System.Text;

namespace WebUI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.TelegramBot(
                token: builder.Configuration["TelegramBot:TelegramApiKey"],
                chatId: builder.Configuration["TelegramBot:TelegramChatId"],
                restrictedToMinimumLevel: LogEventLevel.Warning)
            .CreateLogger();
        try
        {
            Log.Information("Application is started");
            Build(builder);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, ex.Message);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void Build(WebApplicationBuilder builder)
    {        
        builder.Host.UseSerilog();
        builder.Services.AddControllers();
        IConfiguration configuration = builder.Configuration;
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(configuration);
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Authorization", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {{
                new OpenApiSecurityScheme()
                {
                    Reference=new OpenApiReference()
                    {
                        Id="Bearer",
                        Type=ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            } });
        });
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        var app = builder.Build();
        
        app.UseSerilogRequestLogging();        
        app.UseHttpLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseGlobalExeptionHandler();

        app.MapControllers();

        app.Run();
    }
}