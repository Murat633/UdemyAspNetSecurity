using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            var cookies = Request.Cookies.ToList();
            foreach (var cookie in cookies)
            {
                var cookieName = cookie.Key;
                var cookieValue = cookie.Value;

                Console.WriteLine($"{cookieName}:{cookieValue}");
            }
            Response.Cookies.Append("test", "test");
            return Ok(cookies);


        }
    }
}
