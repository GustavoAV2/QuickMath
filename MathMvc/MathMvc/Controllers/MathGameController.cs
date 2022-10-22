using MathMvc.Models;
using MathMvc.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MathMvc.Controllers
{
    public class MathGameController : Controller
    {
        public Random _random { get; set; }
        public MathGameController()
        {
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
        public IActionResult Game([Bind("FirstNumber,LastNumber,Operation,ChallengesSolve,ChallengesUnsolved")] GameModel game, float result)
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

            ViewBag.Game = game;
            return View();
        }
    }
}
