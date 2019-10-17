using ContosoLending.CurrencyExchange.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ContosoLending.CurrencyExchange
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ExchangeRateService>();

                endpoints.MapGet("/proto", async req =>
                    await req.Response.SendFileAsync("Protos/exchange_rate.proto", req.RequestAborted));

                endpoints.MapGet("/", async req => 
                    await req.Response.WriteAsync("Healthy"));
            });
        }
    }
}
