using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yatzy.UnitTests
{
    [TestClass]
    public class ScoreCardTests
    {
        [TestMethod]
        public void ScoreCardReportsBonusWhenBonusObjectivesMet()
        {
            var scoreCard = new ScoreCard(new DefaultObjectives().Objectives);
            for (byte i = 1; i <= 6; i++)
            {
                scoreCard.Score(i - 1, new Roll(new byte[] { i, i, i, i, 0, 0 }));
            }

            scoreCard.TotalScore.Should().Be(84 + 50);
        }

        [TestMethod]
        public void ScoreCardIsCompletedWhenAllObjectivesHaveBeenScored()
        {
            var scoreCard = new ScoreCard(new IObjective[]
            {
                new MockObjective(r => 0),
                new MockObjective(r => 0)
            });

            scoreCard.Score(0, new Roll(new byte[] { 0, 0, 0, 0, 0, 0 }));
            scoreCard.Score(1, new Roll(new byte[] { 0, 0, 0, 0, 0, 0 }));

            scoreCard.Completed.Should().BeTrue();
        }
    }
}
