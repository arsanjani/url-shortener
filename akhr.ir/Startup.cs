using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using akhr.ir.Common;
using akhr.ir.Repos;
using akhr.ir.Repos.Interface;
using akhr.ir.Services;
using akhr.ir.Services.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace akhr.ir
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddMvc();
            services.AddSingleton(_ => Configuration);
            services.AddSingleton(Environment);
            services.AddTransient<IProcessService, ProcessService>();
            services.AddTransient<IProcessRepo, ProcessRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
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
            app.UseMvc();

        }
    }
}
