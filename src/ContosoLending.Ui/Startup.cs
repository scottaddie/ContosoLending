using AutoMapper;
using ContosoLending.Ui.Infrastructure;
using ContosoLending.Ui.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ContosoLending.Ui
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddSingleton<ExchangeRateService>();

            IConfigurationSection loanServiceConfig = Configuration.GetSection("LendingService");

            services.AddHttpClient<LendingService>(config =>
                config.BaseAddress = new Uri(
                    $"{loanServiceConfig["BaseAddress"]}{loanServiceConfig["Routes:HttpStart"]}"));

            var mappingConfig = new MapperConfiguration(config =>
                config.AddProfile(new MappingProfile()));

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
