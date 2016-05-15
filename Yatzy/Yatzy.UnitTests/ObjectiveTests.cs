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
            DefaultObjectives.OnesObjective.Score(roll).Should().Be(6);
        }

        [TestMethod]
        public void TwosObjectiveTest()
        {
            var roll = new Roll(new byte[] { 1, 2, 1, 2, 1, 1 });
            DefaultObjectives.TwosObjective.Score(roll).Should().Be(4);
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
    }
}
