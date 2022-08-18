using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WantACracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Polly;
using System.Net.Http;
using Polly.Contrib.WaitAndRetry;
using Serilog;

namespace WantACracker
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Policy", Version = "v1" });
            });

            // set a normal connection
            services.AddHttpClient<NormalConnection>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44310/");
                // Timeout set at 2 seconds for this example
                client.Timeout = TimeSpan.FromMilliseconds(2000);
            });
            
            // set a exponential backoff using a library, there are other ways to do this using other exension methods
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 5);
            services.AddHttpClient<ExponentialBackoffConnection>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44310/");
            }).AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(delay));

            // set a circuit breaker policy
            services.AddHttpClient<CircuitBreakerConnection>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44310/");
            }).AddTransientHttpErrorPolicy(builder => builder.CircuitBreakerAsync(
                5, // Tries before breaking 
                TimeSpan.FromSeconds(20), // Break for this long
                OnBreak, // Thing to call when broken
                OnReset)); // Function to call when successful requests again

            // set a standard retry policy
            services.AddHttpClient<RetryConnection>(client => 
            { 
                client.BaseAddress = new Uri("https://localhost:44310/");
            }).AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(3)
            }));
        }

        private void OnReset()
        {
            Log.Information("Circuit breaker reset");
        }

        private void OnBreak(DelegateResult<HttpResponseMessage> arg1, TimeSpan arg2)
        {
            Log.Error(arg1.Exception, "Broke at {span}", arg2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Policy v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }
    }
}
