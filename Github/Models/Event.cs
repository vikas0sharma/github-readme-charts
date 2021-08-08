using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReadMeCharts.Github.Models
{
    public class Event
    {
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        public EventType Type { get; set; }
        public Repo Repo { get; set; }
        public Payload Payload { get; set; }
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum EventType
    {
        CommitCommentEvent,
        CreateEvent,
        DeleteEvent,
        ForkEvent,
        GollumEvent,
        IssueCommentEvent,
        IssuesEvent,
        MemberEvent,
        PublicEvent,
        PullRequestEvent,
        PullRequestReviewEvent,
        PullRequestReviewCommentEvent,
        PushEvent,
        ReleaseEvent,
        SponsorshipEvent,
        WatchEvent,
    }
}
