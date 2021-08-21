using RestEase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReadMeCharts.HighChart
{
    public interface IHighChartServerApi
    {
        [Post("highchart-server/draw")]
        Task<Stream> GetChartString([Body] DrawReq request);
    }
    public class DrawReq
    {
        public string Options { get; set; }
    }
}
