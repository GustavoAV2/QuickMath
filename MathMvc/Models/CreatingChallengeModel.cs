using MathMvc.Models.Enums;

namespace MathMvc.Models
{
    public class CreateChallengeModel
    {
        public GameDifficulty Difficulty { get; set; }
        public bool MultipleChoice { get; set; }
    }
}
