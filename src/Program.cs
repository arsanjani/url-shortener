using ScissorLink.Common;
using ScissorLink.Data;
using ScissorLink.Repos;
using ScissorLink.Repos.Interface;
using ScissorLink.Services;
using ScissorLink.Services.Interface;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ScissorLink;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddMemoryCache();
        builder.Services.AddControllers();

        // Add MVC services for views (needed for 404 error page)
        builder.Services.AddControllersWithViews();

        // Add Entity Framework
        builder.Services.AddDbContext<ScissorLinkDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("dbScissorLink")));

        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
        builder.Services.AddTransient<IProcessService, ProcessService>();
        builder.Services.AddTransient<IProcessRepo, ProcessRepo>();

        // Add CORS for React development
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp", policy =>
            {
                policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All,
            RequireHeaderSymmetry = false,
            ForwardLimit = null,
            KnownProxies = { IPAddress.Parse("192.168.0.1"), IPAddress.Parse("192.168.0.2") },
        });

        // Use CORS
        app.UseCors("AllowReactApp");

        app.UseStaticFiles();
        app.UseStatusCodePagesWithReExecute("/error/{0}");
        app.MapControllers();

        app.Run();
    }
}
