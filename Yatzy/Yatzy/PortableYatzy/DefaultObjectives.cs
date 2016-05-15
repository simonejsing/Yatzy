using System.Collections.Generic;
using System.Linq;

namespace Yatzy
{
    public class DefaultObjectives
    {
        public const int YatzyBonus = 50;

        public IObjective[] Objectives { get; private set; }

        public static IObjective Ones => new EyeObjective("Ones", 1);
        public static IObjective Twos => new EyeObjective("Twos", 2);
        public static IObjective Threes => new EyeObjective("Threes", 3);
        public static IObjective Fours => new EyeObjective("Fours", 4);
        public static IObjective Fives => new EyeObjective("Fives", 5);
        public static IObjective Sixes => new EyeObjective("Sixes", 6);
        public static IObjective Pair => new OfAKindObjective("Pair", 2);
        public static IObjective TwoPairs => new TwoPairsObjective();
        public static IObjective ThreePairs => new ThreePairsObjective();
        public static IObjective ThreeOfAKind => new OfAKindObjective("Three of a kind", 3);
        public static IObjective FourOfAKind => new OfAKindObjective("Four of a kind", 4);
        public static IObjective TwoTriplets => new TwoTripletsObjective();
        public static IObjective LowerStraight => new StraightObjective("Lower straight", new byte[] { 1, 2, 3, 4, 5 });
        public static IObjective HigherStraight => new StraightObjective("Higher straight", new byte[] { 2, 3, 4, 5, 6 });
        public static IObjective RoyalStraight => new RoyalStraightObjective();

        public static IObjective FullHouse => new FullHouseObjective();
        public static IObjective Chance => new ChanceObjective();

        public static IObjective Yatzy => new YatzyObjective();

        public DefaultObjectives()
        {
            Objectives = new[]
            {
                Ones,
                Twos,
                Threes,
                Fours,
                Fives,
                Sixes,
                Pair,
                TwoPairs,
                ThreePairs,
                ThreeOfAKind,
                FourOfAKind,
                TwoTriplets,
                LowerStraight,
                HigherStraight,
                RoyalStraight,
                FullHouse,
                Chance,
                Yatzy,
            };
        }

        private class EyeObjective : IObjective
        {
            private readonly int _dice;

            public string Identifier { get; }

            public EyeObjective(string identifier, int dice)
            {
                Identifier = identifier;
                _dice = dice;
            }

            public int Score(Roll roll)
            {
                return roll.Dice.Where(d => d == _dice).Count() * _dice;
            }
        }

        private static IGrouping<byte, byte> MaximumPair(IEnumerable<byte> dice)
        {
            return MaximumOfAKind(dice, 2);
        }

        private static IGrouping<byte, byte> MaximumOfAKind(IEnumerable<byte> dice, int count)
        {
            return dice.GroupBy(d => d).Where(g => g.Count() >= count).OrderByDescending(g => g.Key).FirstOrDefault();
        }

        private class OfAKindObjective : IObjective
        {
            private readonly int _numberOfDice;

            public string Identifier { get; }

            internal OfAKindObjective(string identifier, int numberOfDice)
            {
                Identifier = identifier;
                _numberOfDice = numberOfDice;
            }

            public virtual int Score(Roll roll)
            {
                return MaximumOfAKind(roll.Dice, _numberOfDice)?.Key * _numberOfDice ?? 0;
            }
        }

        private class TwoPairsObjective : IObjective
        {
            public string Identifier => "Two pairs";

            public int Score(Roll roll)
            {
                // Find the first maximum pair
                var firstMaxPair = MaximumPair(roll.Dice);
                if (firstMaxPair == null)
                {
                    return 0;
                }

                // Now remove all dies of this pair from the roll
                var remainingDice = roll.Dice.Where(d => d != firstMaxPair.Key);
                
                // Find the second maximum pair
                var secondMaxPair = MaximumPair(remainingDice);
                if (secondMaxPair == null)
                {
                    return 0;
                }

                return firstMaxPair.Key*2 + secondMaxPair.Key*2;
            }
        }

        private class ThreePairsObjective : IObjective
        {
            public string Identifier => "Three pairs";

            public int Score(Roll roll)
            {
                // Find the first maximum pair
                var firstMaxPair = MaximumPair(roll.Dice);
                if (firstMaxPair == null)
                {
                    return 0;
                }

                // Now remove all dies of the first pair from the roll
                var remainingDice = roll.Dice.Where(d => d != firstMaxPair.Key);

                // Find the second maximum pair
                var secondMaxPair = MaximumPair(remainingDice);
                if (secondMaxPair == null)
                {
                    return 0;
                }

                // Now remove all dies of the second pair from the roll
                remainingDice = remainingDice.Where(d => d != secondMaxPair.Key);

                // Find the third maximum pair
                var thirdMaxPair = MaximumPair(remainingDice);
                if (thirdMaxPair == null)
                {
                    return 0;
                }

                return firstMaxPair.Key * 2 + secondMaxPair.Key * 2 + thirdMaxPair.Key * 2;
            }
        }

        private class TwoTripletsObjective : IObjective
        {
            public string Identifier => "Two triplets";

            public int Score(Roll roll)
            {
                var firstTriplet = MaximumOfAKind(roll.Dice, 3);
                if (firstTriplet == null)
                {
                    return 0;
                }

                // Now remove all dies of the first triplet from the roll
                var remainingDice = roll.Dice.Where(d => d != firstTriplet.Key);

                var secondTriplet = MaximumOfAKind(remainingDice, 3);
                if (secondTriplet == null)
                {
                    return 0;
                }

                return firstTriplet.Key*3 + secondTriplet.Key*3;
            }
        }

        private class StraightObjective : IObjective
        {
            private readonly byte[] _dice;

            public string Identifier { get; }

            public StraightObjective(string identifier, byte[] dice)
            {
                Identifier = identifier;
                _dice = dice;
            }

            public virtual int Score(Roll roll)
            {
                return _dice.Except(roll.Dice).Any() ? 0 : _dice.Sum(d => d);
            }
        }

        private class RoyalStraightObjective : StraightObjective
        {
            public RoyalStraightObjective() : base("Royal straight", new byte[] { 1, 2, 3, 4, 5, 6 })
            {
            }

            public override int Score(Roll roll)
            {
                return base.Score(roll) > 0 ? 30 : 0;
            }
        }

        private class FullHouseObjective : IObjective
        {
            public string Identifier => "Full house";

            public int Score(Roll roll)
            {
                var firstTriplet = MaximumOfAKind(roll.Dice, 3);
                if (firstTriplet == null)
                {
                    return 0;
                }

                // Now remove all dies of the first triplet from the roll
                var remainingDice = roll.Dice.Where(d => d != firstTriplet.Key);

                var secondPair = MaximumOfAKind(remainingDice, 2);
                if (secondPair == null)
                {
                    return 0;
                }

                return firstTriplet.Key * 3 + secondPair.Key * 2;
            }
        }

        private class ChanceObjective : IObjective
        {
            public string Identifier => "Chance";

            public int Score(Roll roll)
            {
                return roll.Dice.Sum(d => d);
            }
        }

        private class YatzyObjective : OfAKindObjective
        {
            public YatzyObjective() : base("Yatzy", 6)
            {
            }

            public override int Score(Roll roll)
            {
                var baseScore = base.Score(roll);
                return baseScore > 0 ? baseScore + YatzyBonus : 0;
            }
        }
    }
}