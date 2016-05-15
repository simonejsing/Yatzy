using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy
{
    class Program
    {
        const int Attempts = 5;

        static void Main(string[] args)
        {
            var dieRoller = new RandomDieRoller();
            int success = 0;

            for (int i = 0; i < Attempts; i++)
            {
                var game = YatzyGameEngine.GetInstance(dieRoller, new DefaultObjectives().Objectives);

                var roll = game.RollDice();
                Console.WriteLine(roll);

                for (int j = 0; j < 2; j++)
                {
                    var hold = HoldStrategy(roll);
                    roll = game.Reroll(hold);
                    Console.WriteLine(roll);
                }

                var points = game.Score(0);
                Console.WriteLine("Score: {0}", points);

                if (points >= 4)
                    success++;
            }

            Console.WriteLine("Success count: {0}", success);
            Console.WriteLine("Success rate: {0}%", (success/(double)Attempts)*100);
        }

        private static int[] HoldStrategy(Roll roll)
        {
            return roll.IndexOfDiceWithValue(1);
        }
    }
}
