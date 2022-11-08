using MathMvc.Models.Enums;

namespace MathMvc.Models
{
    public class GameModel
    {
        public Random _random = new Random();

        public static int MaxChallenges = 5;
        public int ChallengesSolve { get; set; } = 0;
        public int ChallengesUnsolved { get; set; } = 0;
        public int FirstNumber { get; set; }
        public int LastNumber { get; set; }
        public Operation Operation { get; set; }

        public float ActualResult { 
            get {
                switch (Operation)
                {
                    case Operation.Sum:
                        return FirstNumber + LastNumber;
                    case Operation.Subtract:
                        return FirstNumber - LastNumber;
                    case Operation.Division:
                        return FirstNumber / LastNumber;
                    case Operation.Multiply:
                        return FirstNumber * LastNumber;
                    default:
                        return FirstNumber + LastNumber;
                }
            } 
        }
        public int TotalChallenges()
        {
            return ChallengesSolve + ChallengesUnsolved;
        }

        public bool VerifySolution(float number)
        {
            return ActualResult == number;
        }
        public IOrderedEnumerable<float> GeneratorResultsWithFakes()
        {
            var list = new List<float>()
            {
                ActualResult + _random.Next(1000),
                ActualResult + _random.Next(100),
                ActualResult - _random.Next(100),
                ActualResult
            };
            return list.OrderBy(item => _random.Next());
        }
    }

}
