using MathMvc.Models;
using MathMvc.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Security.Claims;
using System.Security.Principal;

namespace MathMvc.Controllers
{
    public class MathGameController : Controller
    {
        public Random _random { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        public MathGameController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _random = new Random();
        }

        public Operation _operationGenerator(int maxOperationNums)
        {
            int operationNumber = _random.Next(1, maxOperationNums);
            switch (operationNumber)
            {
                case 1:
                    return Operation.Sum;
                case 2:
                    return Operation.Subtract;
                case 3:
                    return Operation.Division;
                case 4:
                    return Operation.Multiply;
                default:
                    return Operation.Sum;
            }
        }

        public GameModel GameChallengeGenerator(int challengesSolve = 0, int challengesUnsolved = 0, int maxOperationNums = 2,  List<Operation> disableOperation = null)
        {
            int r1 = _random.Next(1000);
            int r2 = _random.Next(2000);
            Operation op = _operationGenerator(maxOperationNums);
            if (disableOperation != null)
            {
                while (disableOperation.FirstOrDefault(op).Equals(op))
                {
                    op = _operationGenerator(maxOperationNums);
                }
            }
            return new GameModel()
            {
                FirstNumber = r1,
                LastNumber = r2,
                Operation = op,
                ChallengesSolve = challengesSolve,
                ChallengesUnsolved = challengesUnsolved
            };
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Index(GameDifficulty difficulty, bool multiple_choice, bool sum, bool subtraction, bool multiplication, bool division)
        {

            return View();
        }

        public IActionResult TestGame()
        {
            var game = GameChallengeGenerator();
            ViewBag.Game = game;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult TestGame([Bind("FirstNumber,LastNumber,Operation,ChallengesSolve,ChallengesUnsolved")] GameModel game, float result)
        {
            if (game.VerifySolution(result))
            {
                game = GameChallengeGenerator(game.ChallengesSolve + 1, game.ChallengesUnsolved);
            }
            else
            {
                game.ChallengesUnsolved += 1;
                ViewBag.Message = new { Content = "Errouu feio, errou rude!! Tente novamente...", Solve = false };
            }
            if (game.TotalChallenges() == GameModel.MaxChallenges)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Game = game;
            return View();
        }

        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Game([Bind("FirstNumber,LastNumber,Operation,ChallengesSolve,ChallengesUnsolved")] GameModel game, float result)
        {
            if (game.VerifySolution(result))
            {
                game = GameChallengeGenerator(game.ChallengesSolve + 1, game.ChallengesUnsolved);
            }
            else
            {
                game.ChallengesUnsolved += 1;
                ViewBag.Message = new { Content = "Errouu feio, errou rude!! Tente novamente...", Solve = false };
            }
            if (game.TotalChallenges() == GameModel.MaxChallenges)
            {
                string userId = _userManager.GetUserId(HttpContext.User);
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                user.NumberResolvedAccounts += game.ChallengesSolve;
                user.NumberUnresolvedAccounts += game.ChallengesUnsolved;

                // Save Cookies
                HttpContext.Response.Cookies.Append("challengesSolve", game.ChallengesSolve.ToString());
                HttpContext.Response.Cookies.Append("challengesUnsolved", game.ChallengesUnsolved.ToString());

                await _userManager.UpdateAsync(user);
                return RedirectToAction("GameResult");
            }
            ViewBag.Game = game;
            return View();
        }

        [Authorize]
        public IActionResult GameResult()
        {
            var challengesSolve = int.Parse(HttpContext.Request.Cookies["challengesSolve"]);
            var challengesUnsolved = int.Parse(HttpContext.Request.Cookies["challengesUnsolved"]);
            var winRate = (int)(((float)challengesSolve / GameModel.MaxChallenges) * 100);
            ViewData["winRate"] = winRate;
            ViewData["challengesSolve"] = challengesSolve;
            ViewData["challengesUnsolved"] = challengesUnsolved;
            return View();
        }
    }
}
