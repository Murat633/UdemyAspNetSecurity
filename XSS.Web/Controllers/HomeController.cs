using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using XSS.Web.Models;

namespace XSS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public IActionResult CommentAdd()
        {
            HttpContext.Response.Cookies.Append("username", "test");
            HttpContext.Response.Cookies.Append("password", "test");
            if (System.IO.File.Exists("comment.txt"))
            {
                ViewBag.comments = System.IO.File.ReadAllLines("comment.txt");
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CommentAdd(string name, string comment)
        {
            //ViewBag.name = name; 
            //ViewBag.comment = comment;

            await System.IO.File.AppendAllTextAsync("comment.txt", $"{name}-{comment}\n");
            
            return RedirectToAction("commentAdd");
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
