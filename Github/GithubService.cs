using GithubReadMeCharts.Github.Dto;
using GithubReadMeCharts.Github.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReadMeCharts.Github
{
    public class GithubService
    {
        private readonly IGithubApi api;

        public GithubService(IGithubApi api)
        {
            this.api = api;
        }

        public async Task<Repo[]> GetLatestRepos(string user)
        {
            var repos = await api.GetAllRepos(user);
            return repos.Where(r => r.CreatedAt > DateTime.Now.AddYears(-1)).ToArray();
        }

        public async Task<Repo> GetMostStarredRepos(string user)
        {
            var repos = await api.GetAllRepos(user);
            return repos.GroupBy(r => r.Stars).Select(r => r.OrderByDescending(x => x.Stars).First()).First();
        }

        public async Task<Dictionary<string, double>> GetLanguagesStats(string user)
        {
            var repos = await api.GetAllRepos(user);
            var group = repos.Where(r => r.PrimaryLanguage != null)
                .GroupBy(r => r.PrimaryLanguage)
                .Select(g => new
                {
                    Language = g.Key,
                    Count = g.Count()
                });
            var sum = group.Sum(g => g.Count);
            return group.ToDictionary(g => g.Language, g => (double)g.Count / sum * 100);
        }

        public async Task<ActivityDto[]> GetActivityStats(string user)
        {
            var activities = await api.GetAllEvents(user);
            return activities.GroupBy(a => new
            {
                date = a.CreatedAt.Date,
                type = a.Type
            }).Select(g => new ActivityDto
            {
                Date = g.Key.date,
                Type = g.Key.type.ToString(),
                Count = g.Count()
            }).ToArray();
        }

    }
}
