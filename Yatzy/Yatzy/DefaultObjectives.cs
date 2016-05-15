using System.Linq;

namespace Yatzy
{
    public class DefaultObjectives
    {
        public IObjective[] Objectives { get; private set; }

        public static IObjective OnesObjective => new EyeObjective(1);
        public static IObjective TwosObjective => new EyeObjective(2);
        public static IObjective ThreesObjective => new EyeObjective(3);
        public static IObjective FoursObjective => new EyeObjective(4);
        public static IObjective FivesObjective => new EyeObjective(5);
        public static IObjective SixesObjective => new EyeObjective(6);

        public DefaultObjectives()
        {
            Objectives = new[]
            {
                OnesObjective,
                TwosObjective,
                ThreesObjective,
                FoursObjective,
                FivesObjective,
                SixesObjective,
            };
        }

        private class EyeObjective : IObjective
        {
            private readonly int _dice;

            public EyeObjective(int dice)
            {
                _dice = dice;
            }

            public int Score(Roll roll)
            {
                return roll.Dice.Where(d => d == _dice).Count() * _dice;
            }
        }
    }
}