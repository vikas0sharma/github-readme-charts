using GithubReadMeCharts.Github;
using GithubReadMeCharts.HighChart;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GithubReadMeCharts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController(HighChartService highChartService, GithubService githubService)
        {
            this.highChartService = highChartService;
            this.githubService = githubService;
        }
        private readonly IHighChartApi api;
        private readonly HighChartService highChartService;
        private readonly GithubService githubService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var r = await githubService.GetLatestRepos("vikas0sharma");
            var r0 = await githubService.GetLanguagesStats("vikas0sharma");
            var r00 = await highChartService.GetLanguagesUsedChart(r0);
            return File(r00, "image/jpeg");
            /*
            var r1 = await highChartService.GetChartByRepos(r);
            return File(r1, "image/jpeg");
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
            */

        }
    }
}
