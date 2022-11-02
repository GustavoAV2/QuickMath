using MathMvc.Models.Enums;

namespace MathMvc.Models
{
    public class ChallengeModel
    {
        public Random _random = new Random();
        public int FirstNumber { get; set; } = 0;
        public int LastNumber { get; set; } = 0;

        public int ChallengesPlayed { get; set; } = 0;

        public Operation Operation { get; set; }

        public bool VerifySolution(float number)
        {
            return ResultOperation() == number;
        }

        public float ResultOperation()
        {
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

        public IOrderedEnumerable<float> GeneratorResultsWithFakes()
        {
            var result = ResultOperation();
            var list = new List<float>()
            {
                result + _random.Next(1000),
                result + _random.Next(100),
                result - _random.Next(100),
                result
            };
            return list.OrderBy(item => _random.Next());
        }
    }
}
