using GithubReadMeCharts.Github;
using GithubReadMeCharts.HighChart;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestEase;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GithubReadMeCharts
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddScoped(sp => {
                var settings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                };

                var api = new RestClient("http://export.highcharts.com")
                {
                    JsonSerializerSettings = settings
                };
                return api.For<IHighChartApi>();
            });
            services.AddScoped(sp => RestClient.For<IGithubApi>("https://api.github.com"));
            
            services.AddScoped<GithubService>();
            services.AddScoped<HighChartService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            //app.UseAuthorization();

            // add mvc and configure default fallback  to home controller, in order to allow refresh on backend
            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=HomePage}");
            });
        }
    }
}
