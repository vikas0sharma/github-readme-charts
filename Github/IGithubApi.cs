using GithubReadMeCharts.Github.Models;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReadMeCharts.Github
{
    [Header("Accept", "application/vnd.github.v3+json")]
    [Header("User-Agent", "request")]
    public interface IGithubApi
    {
        [Get("users/{user}/repos")]
        Task<Repo[]> GetAllRepos([Path] string user, [Query] int per_page = 100);

        [Get("users/{user}/events")]
        Task<Event[]> GetAllEvents([Path] string user, [Query] int per_page = 100);
    }
}
