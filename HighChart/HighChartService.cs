using GithubReadMeCharts.Github.Dto;
using GithubReadMeCharts.Github.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GithubReadMeCharts.HighChart
{
    public class HighChartService
    {
        private readonly IHighChartApi api;

        public HighChartService(IHighChartApi api)
        {
            this.api = api;
        }

        public async Task<Stream> GetLanguagesUsedChart(Dictionary<string, double> languageScore)
        {
            var request = new
            {
                async = true,
                constr = "Chart",
                infile = new
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
                            stops = new object[] {
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

                },
                scale = false,
                type = "image/png",
                width = false
            };
            var chartLocation = await api.CreateChart(request);

            return await api.GetChart(chartLocation.Replace("charts/", ""));
        }
        public async Task<Stream> GetActivityChart(ActivityDto[] activities)
        {
            var request = new
            {
                async = true,
                constr = "Chart",
                infile = new
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
                            stops = new object[] {
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
                },
                scale = false,
                type = "image/png",
                width = false
            };

            var chartLocation = await api.CreateChart(request);

            return await api.GetChart(chartLocation.Replace("charts/", ""));
        }

        public async Task<Stream> GetActivityTimeline(ActivityDto[] activities)
        {
            var request = new
            {
                async = true,
                constr = "Chart",
                infile = new
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
                },
                scale = false,
                type = "image/png",
                width = false
            };
            var chartLocation = await api.CreateChart(request);

            return await api.GetChart(chartLocation.Replace("charts/", ""));
        }

        public async Task<Stream> GetCommitsWordcloud(Dictionary<string, int> commits)
        {
            var request = new
            {
                async = true,
                constr = "Chart",
                infile = new
                {
                    chart = new
                    {
                        type = "wordcloud",
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
                               data =commits.Select(kv => new { name = kv.Key, weight = kv.Value })

                        }
                    }

                },
                scale = false,
                type = "image/png",
                width = false
            };
            var chartLocation = await api.CreateChart(request);

            return await api.GetChart(chartLocation.Replace("charts/", ""));
        }

        public async Task<Stream> GetChartByRepos(Repo[] repos)
        {
            var series = repos.GroupBy(r => r.CreatedAt.ToString("MMMM"), (key, r) => new { key, repos = r }).Select(g => new
            {
                //name = g.key,
                data = g.repos.Select(r => r.Size),
            });
            var req2 = new
            {
                async = true,
                constr = "Chart",
                infile = new
                {
                    chart = new
                    {
                        type = "column",
                        renderTo = "container",
                        options3d = new
                        {
                            enabled = true,
                            alpha = 15,
                            beta = 15,
                            depth = 50,
                            viewDistance = 25
                        }
                    },
                    title = new
                    {
                        text = "Recent repositories"
                    },
                    series = repos.GroupBy(r => r.CreatedAt.ToString("MMM"), (r, g) => new { repos = g }).Select(gp => gp.repos.Select(r => new { name = r.Name, data = new[] { r.Size } })),
                    //series = new object[] { 
                    //    new
                    //    {
                    //        data = repos.Select(r => r.Size)
                    //    }
                    //},
                    xAxis = new
                    {
                        categories = repos.Select(r => r.CreatedAt.ToString("MMM")).ToArray()
                    }
                },
                scale = false,
                type = "image/png",
                width = false
            };
            var chartLocation = await api.CreateChart(req2);

            return await api.GetChart(chartLocation.Replace("charts/", ""));
        }

    }
}
