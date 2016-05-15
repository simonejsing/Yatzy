using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy
{
    public class Roll
    {
        private readonly byte[] _dice;

        public byte Dice1 => _dice[0];
        public byte Dice2 => _dice[1];
        public byte Dice3 => _dice[2];
        public byte Dice4 => _dice[3];
        public byte Dice5 => _dice[4];
        public byte Dice6 => _dice[5];

        public byte[] Dice => _dice;

        public Roll(byte[] dice)
        {
            if (dice == null)
                throw new ArgumentNullException(nameof(dice));
            if (dice.Length != 6)
                throw new InvalidOperationException("Invalid number of dice in roll.");

            _dice = new byte[6];
            Array.Copy(dice, _dice, 6);
        }

        public Roll(Roll roll)
        {
            _dice = new byte[6];
            Array.Copy(roll.Dice, _dice, 6);
        }

        public int[] IndexOfDiceWithValue(int eyes)
        {
            return Enumerable.Range(0, _dice.Length).Where(index => _dice[index] == eyes).ToArray();
        }

        public override string ToString()
        {
            return string.Join(" ", _dice);
        }
    }
}
