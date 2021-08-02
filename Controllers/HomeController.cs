using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GithubReadMeCharts.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        public IActionResult HomePage() => View();

    }
}
