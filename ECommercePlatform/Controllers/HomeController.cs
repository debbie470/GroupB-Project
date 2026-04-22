using System.Diagnostics; // Provides access to diagnostic tools like Activity IDs for error tracking
using ECommercePlatform.Models; // Grants access to domain models including the ErrorViewModel
using Microsoft.AspNetCore.Mvc; // Imports core ASP.NET Core MVC functionality for controllers and views

namespace ECommercePlatform.Controllers
{
    // Controller responsible for serving the primary non-resource pages (Home, Privacy, Error)
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // Logger instance for recording application events and errors

        // Injects the logger service via the constructor
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: /Home/Index - Returns the main landing page view
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/Privacy - Returns the privacy policy page view
        public IActionResult Privacy()
        {
            return View();
        }

        // Configures the error page to never be cached by the browser or proxies
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Returns the Error view populated with the current Activity ID or Trace Identifier for debugging
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}