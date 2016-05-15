using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy
{
    class Program
    {
        const int Attempts = 5;

        static void Main(string[] args)
        {
            int success = 0;
            var game = YatzyGameEngine.GetInstance();

            for (int i = 0; i < Attempts; i++)
            {
                var roll = game.RollDice();
                Console.WriteLine(roll);

                for (int j = 0; j < 2; j++)
                {
                    var indexOfDiceWithOneEye = GetValue(roll, 1);
                    roll = game.Reroll(indexOfDiceWithOneEye);
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

        private static int[] GetValue(Roll roll, int eyes)
        {
            return Enumerable.Range(0, roll.Dice.Length).Where(index => roll.Dice[index] == eyes).ToArray();
        }
    }
}
