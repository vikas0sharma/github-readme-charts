using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using GithubReadMeCharts.HighChart;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GithubReadMeCharts.Controllers
{
    [Route("highchart-server")]
    [ApiController]
    public class HighChartServerController : Controller
    {
        private readonly IServiceProvider provider;
        private readonly HighChartJsModuleService highChartJsModule;
        private readonly IConfiguration configuration;
        private readonly ILogger logger;

        public HighChartServerController(IServiceProvider provider, HighChartJsModuleService highChartJsModule, IConfiguration configuration, ILogger<HighChartServerController> logger)
        {
            this.provider = provider;
            this.highChartJsModule = highChartJsModule;
            this.configuration = configuration;
            this.logger = logger;
        }

        [HttpPost("draw")]
        public async Task<IActionResult> DrawGraph([FromBody] DrawReq chartOptions)
        {

            var webDriver = provider.GetRequiredService<IWebDriver>();
            try
            {
                logger.LogInformation($"Calling Selenium: {configuration["HighchartServerBaseUrl"]}highchart-server/chart");
                webDriver.Navigate().GoToUrl($"{configuration["HighchartServerBaseUrl"]}highchart-server/chart");
                IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)webDriver;
                var js = $"{highChartJsModule.Js} " +
                    $" drawChart(arguments[0]);";
                javaScriptExecutor.ExecuteScript(js, chartOptions.Options);
                await Task.Delay(2000);
                var imageData = webDriver.FindElement(By.Id("image-data")).GetAttribute("data");
                var base64Data = Regex.Match(imageData, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                var bytes = Convert.FromBase64String(base64Data);
                return File(new MemoryStream(bytes), "image/jpeg");
            }
            catch (Exception e)
            {

            }
            finally
            {
                webDriver.Quit();
            }
            return Ok("done");
        }

        [HttpGet("chart")]
        public IActionResult Chart() => View("_Graph");
    }
}
