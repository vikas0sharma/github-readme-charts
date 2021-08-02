using GithubReadMeCharts.Github;
using GithubReadMeCharts.HighChart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GithubReadMeCharts.Controllers
{
    [ApiController]
    [Route("charts")]
    public class ChartsController : ControllerBase
    {
        private readonly HighChartService highChartService;
        private readonly GithubService githubService;

        public ChartsController(HighChartService highChartService, GithubService githubService)
        {
            this.highChartService = highChartService;
            this.githubService = githubService;
        }

        [HttpGet("languages")]
        public async Task<IActionResult> GetLanguagesStats(string u)
        {
            var data = await githubService.GetLanguagesStats(u);
            var stream = await highChartService.GetLanguagesUsedChart(data);
            return File(stream, "image/jpeg");
        }

        [HttpGet("activity")]
        public async Task<IActionResult> GetActivityStats(string u)
        {
            var data = await githubService.GetActivityStats(u);
            var stream = await highChartService.GetActivityChart(data);
            return File(stream, "image/jpeg");
        }


    }
}
