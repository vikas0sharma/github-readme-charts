using System;

namespace GithubReadMeCharts.Github.Dto
{
    public class ActivityDto
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }
    }
}
