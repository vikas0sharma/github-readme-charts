using GithubReadMeCharts.Github.Dto;
using GithubReadMeCharts.Github.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GithubReadMeCharts.HighChart
{
    public class HighChartService
    {
        private readonly IHighChartServerApi highChartServerApi;

        public HighChartService(IHighChartServerApi highChartServerApi)
        {
            this.highChartServerApi = highChartServerApi;
        }

        public async Task<Stream> GetLanguagesUsedChart(Dictionary<string, double> languageScore)
        {
            var request = new
            {
                chart = new
                {
                    type = "pie",
                    renderTo = "container",
                    options3d = new
                    {
                        enabled = true,
                        alpha = 45,
                    },
                    backgroundColor = new
                    {
                        linearGradient = new[] { 0, 0, 500, 500 },
                        stops = new object[]
                        {
                                new object[]{0, "#2a2a2b" },
                                new object[]{1, "#3e3e40" }
                        }
                    },
                },
                plotOptions = new
                {
                    pie = new
                    {
                        innerSize = 100,
                        depth = 45,
                        dataLabels = new
                        {
                            enabled = true,
                            format = "{point.name}: {point.y:.1f}%",
                            color = "#E0E0E3"
                        }
                    }
                },
                title = new
                {
                    text = "Languages used",
                    style = new
                    {
                        color = "#E0E0E3"
                    }
                },
                series = new[]
                {
                    new
                    {
                        data = languageScore.Select(d => new object[]{ d.Key, d.Value })
                    }
                }

            };

            return await highChartServerApi.GetChartString(new DrawReq
            {
                Options = JsonConvert.SerializeObject(request)
            });
        }
        public async Task<Stream> GetActivityChart(ActivityDto[] activities)
        {
            var request = new
            {
                chart = new
                {
                    type = "spline",
                    renderTo = "container",
                    //options3d = new
                    //{
                    //    enabled = true,
                    //    alpha = 45,
                    //},
                    backgroundColor = new
                    {
                        linearGradient = new[] { 0, 0, 500, 500 },
                        stops = new object[]
                        {
                                new object[]{0, "#2a2a2b" },
                                new object[]{1, "#3e3e40" }
                        }
                    },
                },
                plotOptions = new
                {
                    series = new
                    {
                        dataLabels = new
                        {
                            enabled = true,
                            color = "#E0E0E3"
                        }
                    }
                },
                title = new
                {
                    text = "Activities Heartbeat",
                    style = new
                    {
                        color = "#E0E0E3"
                    }
                },
                series = activities.GroupBy(a => a.Type, (k, a) => new { Key = k, Act = a })
                    .Select(a => new
                    {
                        name = a.Key,
                        data = a.Act.Select(x => new object[] { $"Date.UTC({x.Date.Year},{x.Date.Month}, {x.Date.Day })", x.Count })
                    }),
                legend = new
                {
                    itemStyle = new
                    {
                        font = "9pt Trebuchet MS, Verdana, sans-serif",
                        color = "white"
                    },
                },
                xAxis = new
                {
                    type = "datetime",
                    dateTimeLabelFormats = new
                    { // don't display the dummy year
                        month = "%e. %b",
                        year = "%Y"
                    },
                    title = new
                    {
                        text = "Date"
                    }
                },
            };

            return await highChartServerApi.GetChartString(new DrawReq
            {
                Options = JsonConvert.SerializeObject(request)
            });
        }

        public async Task<Stream> GetActivityTimeline(ActivityDto[] activities)
        {
            var request = new
            {
                chart = new
                {
                    type = "timeline",
                    renderTo = "container",
                    //options3d = new
                    //{
                    //    enabled = true,
                    //    alpha = 45,
                    //},
                    //backgroundColor = new
                    //{
                    //    linearGradient = new[] { 0, 0, 500, 500 },
                    //    stops = new object[] {
                    //        new object[] { 0, "#2a2a2b" },
                    //        new object[] { 1, "#3e3e40" }
                    //    }
                    //},
                },
                //plotOptions = new
                //{
                //    series = new
                //    {
                //        dataLabels = new
                //        {
                //            enabled = true,
                //            color = "#E0E0E3"
                //        }
                //    }
                //},
                title = new
                {
                    text = "Repositories Timeline",
                    //style = new
                    //{
                    //    color = "#E0E0E3"
                    //}
                },
                series = new object[] {
                       new {
                               data =
                               activities.Select(a => new
                               {
                                name = $"<b>{a.Date.ToShortDateString()}</b>",
                                label = $"<i>{a.Repo}</i>"
                               }),
                               type = "timeline"
                        }
                    },
                xAxis = new
                {
                    visible = false,
                },
                yAxis = new
                {
                    visible = false
                },
                colors = new string[] {
                        "#4185F3",
                        "#427CDD",
                        "#406AB2",
                        "#3E5A8E",
                        "#3B4A68",
                        "#363C46"
                    },
            };

            return await highChartServerApi.GetChartString(new DrawReq
            {
                Options = JsonConvert.SerializeObject(request)
            });
        }

        public async Task<Stream> GetCommitsWordcloud(Dictionary<string, int> commits)
        {
            var request = new
            {
                chart = new
                {
                    type = "wordcloud",
                    renderTo = "container",
                },
                title = new
                {
                    text = "Comments Wordcloud",
                },
                series = new object[]
                {
                    new
                    {
                        data = commits.Select(kv => new { name = kv.Key, weight = kv.Value })
                    }
                }
            };

            return await highChartServerApi.GetChartString(new DrawReq
            {
                Options = JsonConvert.SerializeObject(request)
            });
        }

    }
}
