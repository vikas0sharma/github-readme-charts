using Microsoft.AspNetCore.Mvc;

namespace GithubReadMeCharts.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        public IActionResult HomePage() => View();
    }
}
