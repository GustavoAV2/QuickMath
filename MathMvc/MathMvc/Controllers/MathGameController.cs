using MathMvc.Models;
using MathMvc.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MathMvc.Controllers
{
    public class MathGameController : Controller
    {
        public Random _random { get; set; }
        private readonly UserManager<IdentityUser> _userManager;
        public MathGameController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _random = new Random();
        }

        public Operation _operationGenerator(int operationNumber)
        {
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

        public GameModel GameChallengeGenerator(int challengesSolve = 0, int challengesUnsolved = 0)
        {
            int r1 = _random.Next(1000);
            int r2 = _random.Next(2000);
            int r3 = _random.Next(1,2);
            return new GameModel()
            {
                FirstNumber = r1,
                LastNumber = r2,
                Operation = _operationGenerator(r3),
                ChallengesSolve = challengesSolve,
                ChallengesUnsolved = challengesUnsolved
            };
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Game()
        {
            var game = GameChallengeGenerator();
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
            if (game.TotalChallenges() == game.MaxChallenges)
            {
                var email = ClaimTypes.Email;
                //ApplicationUser user = await _userManager.FindByEmailAsync(email);
                return View();
            }
            ViewBag.Game = game;
            return View();
        }

        [Authorize]
        public IActionResult GameResult(int challengesSolve, int challengesUnsolved)
        {
            var winRate = challengesSolve / challengesUnsolved;
            ViewData["winRate"] = winRate;
            ViewData["challengesSolve"] = challengesSolve;
            ViewData["challengesUnsolved"] = challengesUnsolved;
            return View();
        }
    }
}
