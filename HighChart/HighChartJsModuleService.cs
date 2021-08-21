using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;

namespace GithubReadMeCharts.HighChart
{
    public class HighChartJsModuleService
    {
        private string js = null;
        public string Js => js ??= File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),"HighChart", "highchart-export.js"));
    }
}
