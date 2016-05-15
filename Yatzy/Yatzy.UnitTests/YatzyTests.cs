using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yatzy.UnitTests
{
    [TestClass]
    public class YatzyTests
    {
        private static YatzyGameEngine CreateDefaultInstance(byte[] expectedDieValue)
        {
            var mockDiceRoller = new MockDiceRoller(expectedDieValue);
            var mockObjectives = new IObjective[] { new MockObjective(r => 0) };
            var game = YatzyGameEngine.GetInstance(mockDiceRoller, mockObjectives);
            return game;
        }

        private static YatzyGameEngine CreateCustomInstance(byte[] expectedDieValue, params IObjective[] objectives)
        {
            var mockDiceRoller = new MockDiceRoller(expectedDieValue);
            var game = YatzyGameEngine.GetInstance(mockDiceRoller, objectives);
            return game;
        }

        [TestMethod]
        public void YatzyGameEngineGetInstanceThrowsWhenDiceRollerIsNull()
        {
            Action action = () => YatzyGameEngine.GetInstance(null, new IObjective[] { new MockObjective(r => 0) });
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void YatzyGameEngineGetInstanceThrowsWhenObjectivesListIsNull()
        {
            Action action = () => YatzyGameEngine.GetInstance(new MockDiceRoller(new byte[]{0}), null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void YatzyGameEngineCanRollDice()
        {
            byte[] expectedDieValue = { 1,2,3,4,5,6 };

            var game = CreateDefaultInstance(expectedDieValue);
            var roll = game.RollDice();

            roll.Dice1.Should().Be(expectedDieValue[0]);
            roll.Dice2.Should().Be(expectedDieValue[1]);
            roll.Dice3.Should().Be(expectedDieValue[2]);
            roll.Dice4.Should().Be(expectedDieValue[3]);
            roll.Dice5.Should().Be(expectedDieValue[4]);
            roll.Dice6.Should().Be(expectedDieValue[5]);
        }

        [TestMethod]
        public void YatzyGameEngineCanRerollAllDice()
        {
            byte[] expectedDieValue = { 1, 2, 3, 4, 5, 6, 6, 5, 4, 3, 2, 1 };

            var game = CreateDefaultInstance(expectedDieValue);
            game.RollDice();
            var newRoll = game.Reroll();

            newRoll.Dice1.Should().Be(expectedDieValue[6]);
            newRoll.Dice2.Should().Be(expectedDieValue[7]);
            newRoll.Dice3.Should().Be(expectedDieValue[8]);
            newRoll.Dice4.Should().Be(expectedDieValue[9]);
            newRoll.Dice5.Should().Be(expectedDieValue[10]);
            newRoll.Dice6.Should().Be(expectedDieValue[11]);
        }

        [TestMethod]
        public void YatzyGameEngineCanHoldFirstDieOnReroll()
        {
            byte[] expectedDieValue = { 1, 2, 3, 4, 5, 6, 6, 5, 4, 3, 2, 1 };

            var game = CreateDefaultInstance(expectedDieValue);
            game.RollDice();
            var newRoll = game.Reroll(0);

            newRoll.Dice1.Should().Be(expectedDieValue[0]);
            newRoll.Dice2.Should().Be(expectedDieValue[6]);
            newRoll.Dice3.Should().Be(expectedDieValue[7]);
            newRoll.Dice4.Should().Be(expectedDieValue[8]);
            newRoll.Dice5.Should().Be(expectedDieValue[9]);
            newRoll.Dice6.Should().Be(expectedDieValue[10]);
        }

        [TestMethod]
        public void YatzyGameEngineCanRerollMaximumTwoTimes()
        {
            byte[] expectedDieValue = { 1, 2, 3, 4, 5, 6 };

            var game = CreateDefaultInstance(expectedDieValue);
            game.RollDice();
            game.Reroll();
            game.Reroll();

            // On a thrid reroll the game should throw an exception
            Action action = () => game.Reroll();
            action.ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void YatzyGameEngineCanRerollAgainAfterANewRoll()
        {
            byte[] expectedDieValue = { 1, 2, 3, 4, 5, 6 };

            var game = CreateDefaultInstance(expectedDieValue);
            game.RollDice();
            game.Reroll();
            game.Reroll();

            game.RollDice();
            game.Reroll();
            game.Reroll();
        }

        [TestMethod]
        public void YatzyGameEngineCannotRerollBeforeDiceHaveBeenRolled()
        {
            byte[] expectedDieValue = { 1, 2, 3, 4, 5, 6 };

            var game = CreateDefaultInstance(expectedDieValue);

            Action action = () => game.Reroll();
            action.ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void YatzyGameEngineCannotScoreBeforeDiceHaveBeenRolled()
        {
            byte[] expectedDieValue = { 1, 2, 3, 4, 5, 6 };

            var game = CreateDefaultInstance(expectedDieValue);

            Action action = () => game.Score(0);
            action.ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void YatzyGameEngineMustRollDiceAgainBeforePlayerCanScoreAgain()
        {
            byte[] expectedDieValue = { 1, 2, 3, 4, 5, 6 };

            var game = CreateDefaultInstance(expectedDieValue);
            game.RollDice();
            game.Score(0);

            Action action = () => game.Score(0);
            action.ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void YatzyGameEngineCanScoreAnObjective()
        {
            const int expectedPoints = 42;
            byte[] expectedDieValue = { 1, 2, 3, 4, 5, 6 };

            var game = CreateCustomInstance(expectedDieValue, new MockObjective(r => expectedPoints));
            game.RollDice();
            var points = game.Score(0);

            points.Should().Be(expectedPoints);
        }
    }
}
