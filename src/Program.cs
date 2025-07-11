using akhr.ir.Common;
using akhr.ir.Repos;
using akhr.ir.Repos.Interface;
using akhr.ir.Services;
using akhr.ir.Services.Interface;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;

namespace akhr.ir;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddMemoryCache();
        builder.Services.AddControllers();
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
        builder.Services.AddTransient<IProcessService, ProcessService>();
        builder.Services.AddTransient<IProcessRepo, ProcessRepo>();

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
            KnownProxies = { IPAddress.Parse("10.70.70.11"), IPAddress.Parse("10.100.1.130") },
        });

        app.UseStaticFiles();
        app.UseStatusCodePagesWithReExecute("/error/{0}");
        app.MapControllers();

        app.Run();
    }
}
