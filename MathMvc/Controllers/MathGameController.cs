using MathMvc.Models;
using MathMvc.Models.Enums;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
=======
using RestSharp;
using System.Security.Claims;
using System.Security.Principal;
>>>>>>> 035134c6a68bf7d4d7657daf1daa2d172cc3f91c

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

<<<<<<< HEAD
        public int GetNumOfOperationsByName(string name = "")
        {
            if (name == "Easy")
            {
                return 1;
            }
            else if (name == "Hard")
            {
                return 3;
            }
            else if (name == "Genius")
            {
                return 4;
            }
            return 2;
        }

        public Operation _operationGenerator(int maxOperationNums)
        {
=======
        public Operation _operationGenerator(int maxOperationNums)
        {
>>>>>>> 035134c6a68bf7d4d7657daf1daa2d172cc3f91c
            int operationNumber = _random.Next(1, maxOperationNums);
            switch (operationNumber)
            {
                case 1:
                    return Operation.Sum;
                case 2:
                    return Operation.Subtract;
                case 3:
                    return Operation.Multiply;
                case 4:
                    return Operation.Division;
                default:
                    return Operation.Sum;
            }
        }

<<<<<<< HEAD
        public GameModel GameChallengeGenerator(int challengesSolve = 0, int challengesUnsolved = 0, int maxOperationNums = 2)
        {
            Operation op = _operationGenerator(maxOperationNums);
            return new GameModel(challengesSolve, challengesUnsolved, op);
=======
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
>>>>>>> 035134c6a68bf7d4d7657daf1daa2d172cc3f91c
        }
        public IActionResult Index(int tag = 0)
        {
            var _createModel = new CreateChallengeModel();
            if (tag <= (int)GameDifficulty.None || tag > (int)GameDifficulty.Genius)
            {
                _createModel.Difficulty = GameDifficulty.Normal;
                return View(_createModel);
            }
            _createModel.Difficulty = (GameDifficulty)tag;
            return View(_createModel);
        }

        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
<<<<<<< HEAD
        public IActionResult Index([Bind("Difficulty, MultipleChoice")] CreateChallengeModel createModel)
=======
        public IActionResult Index(GameDifficulty difficulty, bool multiple_choice, bool sum, bool subtraction, bool multiplication, bool division)
>>>>>>> 035134c6a68bf7d4d7657daf1daa2d172cc3f91c
        {
            HttpContext.Response.Cookies.Append("difficulty", createModel.Difficulty.ToString());
            return RedirectToAction("Game", "MathGame");
        }

<<<<<<< HEAD
        public IActionResult Game()
=======
        public IActionResult TestGame()
>>>>>>> 035134c6a68bf7d4d7657daf1daa2d172cc3f91c
        {
            string? difficultyLevel = HttpContext?.Request?.Cookies?["difficulty"];
            var numOfoperations = GetNumOfOperationsByName(difficultyLevel);
            var game = GameChallengeGenerator(maxOperationNums: numOfoperations);
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
            var difficultyLevel = HttpContext.Request.Cookies["difficulty"];
            var numOfoperations = GetNumOfOperationsByName(difficultyLevel);
            if (game.VerifySolution(result))
            {
                game = GameChallengeGenerator(game.ChallengesSolve + 1, game.ChallengesUnsolved, numOfoperations);
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
<<<<<<< HEAD
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
=======
>>>>>>> 035134c6a68bf7d4d7657daf1daa2d172cc3f91c
            return View();
        }
    }
}
