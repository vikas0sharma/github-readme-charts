using GithubReadMeCharts.Github;
using GithubReadMeCharts.HighChart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger logger;

        public ChartsController(HighChartService highChartService, GithubService githubService, ILogger<ChartsController> logger)
        {
            this.highChartService = highChartService;
            this.githubService = githubService;
            this.logger = logger;
        }

        [HttpGet("languages")]
        public async Task<IActionResult> GetLanguagesStats(string u)
        {
            var data = await githubService.GetLanguagesStats(u);
            var stream = await highChartService.GetLanguagesUsedChart(data);
            return File(stream, "image/png");
        }

        [HttpGet("activity")]
        public async Task<IActionResult> GetActivityStats(string u)
        {
            var data = await githubService.GetActivityStats(u);
            var stream = await highChartService.GetActivityChart(data);
            return File(stream, "image/png");
        }

        [HttpGet("timeline")]
        public async Task<IActionResult> GetRepoTime(string u)
        {
            var data = await githubService.GetActivitiesByType(u, Github.Models.EventType.CreateEvent);
            var stream = await highChartService.GetActivityTimeline(data);
            return File(stream, "image/png");
        }

        [HttpGet("wordcloud")]
        public async Task<IActionResult> GetCommitsWordcloud(string u)
        {
            logger.LogInformation("Calling Wordcloud >>>>>>>>>>>>>>>>>>>>>>>>>>>");
            var data = await githubService.GetCommitsWeightage(u);
            var stream = await highChartService.GetCommitsWordcloud(data);
            return File(stream, "image/png");
        }

    }
}
