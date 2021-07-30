using RestEase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReadMeCharts.HighChart
{
    public interface IHighChartApi
    {
        [Post("")]
        Task<string> CreateChart([Body]object obj);

        [Get("charts/{id}")]
        Task<Stream> GetChart([Path]string id);
    }
}
