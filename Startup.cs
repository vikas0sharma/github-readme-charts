using GithubReadMeCharts.Github;
using GithubReadMeCharts.HighChart;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using RestEase;
using RestEase.HttpClientFactory;
using System;
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
            services.AddHttpClient("example")
                .ConfigureHttpClient(x => x.BaseAddress = new Uri($"{Configuration["HighchartServerBaseUrl"]}"))
                .UseWithRestEaseClient<IHighChartServerApi>();

            //services.AddScoped(sp => RestClient.For<IHighChartServerApi>($"{Configuration["HighchartServerBaseUrl"]}/highchart-server"));
            services.AddScoped(sp => RestClient.For<IGithubApi>("https://api.github.com"));
            
            services.AddScoped<GithubService>();
            services.AddScoped<HighChartService>();
            // Register IWebDriver dependency
            services.AddScoped<IWebDriver>(sp => {
                var opt = new ChromeOptions();
                //opt.AddUserProfilePreference("download.default_directory", dataOutDirectory.Replace("/", "\\"));
                //opt.AddArgument("--force-device-scale-factor=1");
                //opt.AddArgument("--start-maximized");
                opt.AddArguments("headless");
                var driver = new ChromeDriver(opt);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);// default
                ((RemoteWebDriver)driver).FileDetector = new LocalFileDetector();

                return driver;
            });
            services.AddSingleton<HighChartJsModuleService>();
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
