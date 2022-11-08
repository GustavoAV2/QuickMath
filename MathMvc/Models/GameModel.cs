using MathMvc.Models.Enums;

namespace MathMvc.Models
{
    public class GameModel
    {
        public Random _random = new Random();
        public int HardNumber = 100;
        public int NormalNumber = 1000;

        public static int MaxChallenges = 5;
        public int ChallengesSolve { get; set; } = 0;
        public int ChallengesUnsolved { get; set; } = 0;
        public int FirstNumber { get; set; }
        public int LastNumber { get; set; }
        public Operation Operation { get; set; }

        public GameModel()
        {}

        public GameModel(int challengesSolve, int challengesUnsolved, Operation operation)
        {
            ChallengesSolve = challengesSolve;
            ChallengesUnsolved = challengesUnsolved;
            Operation = operation;
            FirstNumber = GenerateValueByOperation(operation);
            LastNumber = GenerateValueByOperation(operation, FirstNumber);
        }

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
        private int GenerateValueByOperation(Operation op, int limitNumber = 0)
        {
            if (op.Equals(Operation.Sum) || op.Equals(Operation.Subtract))
            {
                limitNumber = NormalNumber;
            }
            else if (limitNumber == 0)
            {
                limitNumber = HardNumber;
            }
            return _random.Next(1, limitNumber);
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
