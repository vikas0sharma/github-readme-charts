using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReadMeCharts.Github.Models
{
    public class Repo
    {
        public string Name { get; set; }

        [JsonProperty("forks_count")]
        public int ForksCount { get; set; }

        [JsonProperty("stargazers_count")]
        public int Stars { get; set; }
        
        [JsonProperty("language")]
        public string PrimaryLanguage { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        public int Size { get; set; }
    }
}
