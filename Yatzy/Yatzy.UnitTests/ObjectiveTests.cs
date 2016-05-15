using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yatzy.UnitTests
{
    [TestClass]
    public class ObjectiveTests
    {
        [TestMethod]
        public void OnesObjectiveTest()
        {
            var roll = new Roll(new byte[] { 1, 1, 1, 1, 1, 1 });
            DefaultObjectives.Ones.Score(roll).Should().Be(6);
        }

        [TestMethod]
        public void TwosObjectiveTest()
        {
            var roll = new Roll(new byte[] { 1, 2, 1, 2, 1, 1 });
            DefaultObjectives.Twos.Score(roll).Should().Be(4);
        }

        [TestMethod]
        public void EyeObjectivesTotalOfEightyFour()
        {
            var defaultObjectives = new DefaultObjectives();
            int score = 0;

            for (byte i = 1; i <= 6; i++)
            {
                var roll = new Roll(new byte[] { i, i, i, i, 0, 0 });
                score += defaultObjectives.Objectives[i - 1].Score(roll);
            }

            score.Should().Be(84);
        }

        [TestMethod]
        public void PairObjectiveTest()
        {
            var roll = new Roll(new byte[] { 1, 2, 3, 4, 5, 6 });
            DefaultObjectives.Pair.Score(roll).Should().Be(0);

            roll = new Roll(new byte[] { 1, 2, 1, 2, 4, 6 });
            DefaultObjectives.Pair.Score(roll).Should().Be(4);

            roll = new Roll(new byte[] { 5, 2, 1, 2, 5, 1 });
            DefaultObjectives.Pair.Score(roll).Should().Be(10);

            roll = new Roll(new byte[] { 5, 2, 2, 2, 5, 1 });
            DefaultObjectives.Pair.Score(roll).Should().Be(10);

            roll = new Roll(new byte[] { 5, 2, 2, 2, 5, 5 });
            DefaultObjectives.Pair.Score(roll).Should().Be(10);
        }

        [TestMethod]
        public void TwoPairsObjectiveTest()
        {
            var roll = new Roll(new byte[] { 1, 1, 2, 3, 4, 6 });
            DefaultObjectives.TwoPairs.Score(roll).Should().Be(0);

            roll = new Roll(new byte[] { 1, 2, 1, 2, 4, 6 });
            DefaultObjectives.TwoPairs.Score(roll).Should().Be(6);

            roll = new Roll(new byte[] { 1, 2, 1, 2, 4, 2 });
            DefaultObjectives.TwoPairs.Score(roll).Should().Be(6);

            roll = new Roll(new byte[] { 1, 2, 1, 2, 5, 5 });
            DefaultObjectives.TwoPairs.Score(roll).Should().Be(14);

            roll = new Roll(new byte[] { 2, 2, 2, 2, 1, 3 });
            DefaultObjectives.TwoPairs.Score(roll).Should().Be(0);
        }

        [TestMethod]
        public void ThreePairsObjectiveTest()
        {
            var roll = new Roll(new byte[] { 1, 1, 2, 2, 4, 6 });
            DefaultObjectives.ThreePairs.Score(roll).Should().Be(0);

            roll = new Roll(new byte[] { 1, 1, 2, 2, 4, 4 });
            DefaultObjectives.ThreePairs.Score(roll).Should().Be(14);

            roll = new Roll(new byte[] { 2, 2, 2, 2, 4, 4 });
            DefaultObjectives.ThreePairs.Score(roll).Should().Be(0);
        }

        [TestMethod]
        public void ThreeOfAKindObjectiveTest()
        {
            var roll = new Roll(new byte[] { 1, 1, 2, 3, 4, 6 });
            DefaultObjectives.ThreeOfAKind.Score(roll).Should().Be(0);

            roll = new Roll(new byte[] { 2, 2, 2, 3, 4, 6 });
            DefaultObjectives.ThreeOfAKind.Score(roll).Should().Be(6);

            roll = new Roll(new byte[] { 2, 2, 2, 2, 4, 6 });
            DefaultObjectives.ThreeOfAKind.Score(roll).Should().Be(6);

            roll = new Roll(new byte[] { 2, 2, 2, 3, 3, 3 });
            DefaultObjectives.ThreeOfAKind.Score(roll).Should().Be(9);
        }

        [TestMethod]
        public void FourOfAKindObjectiveTest()
        {
            var roll = new Roll(new byte[] { 1, 1, 1, 3, 4, 6 });
            DefaultObjectives.FourOfAKind.Score(roll).Should().Be(0);

            roll = new Roll(new byte[] { 2, 2, 2, 2, 4, 6 });
            DefaultObjectives.FourOfAKind.Score(roll).Should().Be(8);

            roll = new Roll(new byte[] { 2, 2, 2, 2, 2, 6 });
            DefaultObjectives.FourOfAKind.Score(roll).Should().Be(8);
        }

        [TestMethod]
        public void TwoTripletsObjectiveTest()
        {
            var roll = new Roll(new byte[] { 1, 1, 1, 2, 4, 6 });
            DefaultObjectives.TwoTriplets.Score(roll).Should().Be(0);

            roll = new Roll(new byte[] { 1, 1, 1, 4, 4, 4 });
            DefaultObjectives.TwoTriplets.Score(roll).Should().Be(15);

            roll = new Roll(new byte[] { 1, 1, 1, 1, 1, 1 });
            DefaultObjectives.TwoTriplets.Score(roll).Should().Be(0);
        }

        [TestMethod]
        public void FullHouseObjectiveTest()
        {
            var roll = new Roll(new byte[] { 1, 1, 1, 2, 4, 6 });
            DefaultObjectives.FullHouse.Score(roll).Should().Be(0);

            roll = new Roll(new byte[] { 1, 1, 1, 4, 4, 5 });
            DefaultObjectives.FullHouse.Score(roll).Should().Be(11);

            roll = new Roll(new byte[] { 1, 1, 1, 4, 4, 4 });
            DefaultObjectives.FullHouse.Score(roll).Should().Be(14);

            roll = new Roll(new byte[] { 1, 1, 1, 1, 1, 1 });
            DefaultObjectives.FullHouse.Score(roll).Should().Be(0);
        }

        [TestMethod]
        public void LowerStraightObjectiveTest()
        {
            var roll = new Roll(new byte[] { 2, 1, 3, 4, 5, 2 });
            DefaultObjectives.LowerStraight.Score(roll).Should().Be(15);

            roll = new Roll(new byte[] { 2, 1, 3, 6, 5, 2 });
            DefaultObjectives.LowerStraight.Score(roll).Should().Be(0);
        }

        [TestMethod]
        public void HigherStraightObjectiveTest()
        {
            var roll = new Roll(new byte[] { 2, 6, 3, 4, 5, 2 });
            DefaultObjectives.HigherStraight.Score(roll).Should().Be(20);

            roll = new Roll(new byte[] { 2, 1, 3, 6, 5, 2 });
            DefaultObjectives.HigherStraight.Score(roll).Should().Be(0);
        }

        [TestMethod]
        public void RoyalStraightObjectiveTest()
        {
            var roll = new Roll(new byte[] { 1, 6, 3, 4, 5, 2 });
            DefaultObjectives.RoyalStraight.Score(roll).Should().Be(30);

            roll = new Roll(new byte[] { 2, 1, 3, 6, 5, 2 });
            DefaultObjectives.RoyalStraight.Score(roll).Should().Be(0);
        }

        [TestMethod]
        public void ChanceObjectiveTest()
        {
            var roll = new Roll(new byte[] { 1, 1, 1, 2, 4, 6 });
            DefaultObjectives.Chance.Score(roll).Should().Be(15);
        }

        [TestMethod]
        public void YatzyObjectiveTest()
        {
            var roll = new Roll(new byte[] { 1, 1, 1, 3, 4, 6 });
            DefaultObjectives.Yatzy.Score(roll).Should().Be(0);

            roll = new Roll(new byte[] { 2, 2, 2, 2, 2, 2 });
            DefaultObjectives.Yatzy.Score(roll).Should().Be(12 + DefaultObjectives.YatzyBonus);
        }
    }
}
