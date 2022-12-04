using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MathMvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

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

        [Route("identity")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Identity()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ApplicationUser user = _userManager.FindByIdAsync(userId).Result;
            if (user.NumberUnresolvedAccounts > 0)
            {
                int resolved = user.NumberResolvedAccounts;
                int unresolved = user.NumberUnresolvedAccounts;
                int totalSolutions = resolved + unresolved;
                ViewData["WinRate"] = (int)(((float)resolved / totalSolutions) * 100);
            }
            else
            {
                ViewData["WinRate"] = "100";
            }
            return View(user);
        }

        [Route("identity/friend")]
        [AllowAnonymous]
        public async Task<IActionResult> IdentityFriend(string? email, string? name)
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ApplicationUser user = _userManager.FindByIdAsync(userId).Result;
            user.Friends = user.Friends != null ? user.Friends : new List<Friend>();
            ApplicationUser userFound = null;

            if (!String.IsNullOrEmpty(name))
            {
                userFound = await _userManager.FindByNameAsync(name);
            }
            if (!String.IsNullOrEmpty(email))
            {
                userFound = await _userManager.FindByEmailAsync(email);
            }
            if (userFound != null)
            {
                if (userFound.NumberUnresolvedAccounts > 0)
                {
                    int resolved = userFound.NumberResolvedAccounts;
                    int unresolved = userFound.NumberUnresolvedAccounts;
                    int totalSolutions = resolved + unresolved;
                    ViewData["WinRate"] = (int)(((float)resolved / totalSolutions) * 100);
                }
                else
                {
                    ViewData["WinRate"] = "100";
                }
            }

            ViewBag.userFound = userFound;
            return View(user);
        }

        [Route("add/friend")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddFriend(string friendId)
        {

            string userId = _userManager.GetUserId(HttpContext.User);
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            ApplicationUser userFound = null;
            if (!String.IsNullOrEmpty(friendId) && user != null)
            {
                userFound = await _userManager.FindByIdAsync(friendId);
            }
            if (userFound == null)
            {
                return View("Couldn't send invite");
            }
            Friend friend = new (userFound.Id, userFound.FirstName, userFound.LastName);
            if (user.Friends == null)
            {
                user.Friends = new List<Friend>();
            }
            user.Friends.Add(friend);
            await _userManager.UpdateAsync(user);
            return RedirectToAction("IdentityFriend", new {email = userFound .Email});
        }

        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}