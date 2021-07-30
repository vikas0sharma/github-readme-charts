using GithubReadMeCharts.HighChart;
using IronPython.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GithubReadMeCharts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController(IHighChartApi api)
        {
            this.api = api;
        }
        private readonly IHighChartApi api;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var res = await api.CreateChart(new
            {
                async = true,
                constr = "Chart",
                infile = new
                {
                    series = new object[]
                     {
                        new
                        {
                            data = new []{1,3,5},
                            type="line"
                        },
                        new
                        {
                            data = new []{6,8,3},
                            type="line"
                        }
                     },
                    xAxis = new
                    {
                        categories = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }
                    }
                },
                scale = false,
                type = "image/png",
                width = false
            });

            var resss = await api.GetChart(res.Replace("charts/",""));
            return File(resss, "image/jpeg");
           
        }
    }
}
