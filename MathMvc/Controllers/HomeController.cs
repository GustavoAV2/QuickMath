using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MathMvc.Models;
using Microsoft.AspNetCore.Identity;

namespace MathMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Identity()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ApplicationUser user = _userManager.FindByIdAsync(userId).Result;
            ViewData["WinRate"] = (float)user.NumberResolvedAccounts / user.NumberUnresolvedAccounts;
            return View(user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}