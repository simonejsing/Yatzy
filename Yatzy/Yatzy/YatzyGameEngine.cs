﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy
{
    public class YatzyGameEngine
    {
        private readonly IDiceRoller _diceRoller;
        private readonly IObjective[] _objectives;

        private Roll _currentRoll = null;
        private int _rerollCount;

        public static YatzyGameEngine GetInstance(IDiceRoller diceRoller, IObjective[] objectives)
        {
            return new YatzyGameEngine(diceRoller, objectives);
        }

        public static YatzyGameEngine GetInstance()
        {
            return new YatzyGameEngine(new RandomDieRoller(), DefaultObjectives());
        }

        private static IObjective[] DefaultObjectives()
        {
            return new DefaultObjectives().Objectives;
        }

        private YatzyGameEngine(IDiceRoller diceRoller, IObjective[] objectives)
        {
            if(diceRoller == null)
                throw new ArgumentNullException(nameof(diceRoller));
            if(objectives == null)
                throw new ArgumentNullException(nameof(objectives));

            _diceRoller = diceRoller;
            _objectives = objectives;
        }

        public Roll RollDice()
        {
            var roll = new byte[6];
            _rerollCount = 0;

            for (var i = 0; i < 6; i++)
            {
                roll[i] = _diceRoller.RollDie();
            }

            _currentRoll = new Roll(roll);
            return _currentRoll;
        }

        public Roll Reroll(params int[] hold)
        {
            if(_currentRoll == null)
                throw new InvalidOperationException("Cannot reroll dice before initial roll has been made.");
            _rerollCount++;

            if(_rerollCount > 2)
                throw new InvalidOperationException("Maximum number of rerolls allowed is two.");

            var originalDice = _currentRoll.Dice;

            for (int i = 0; i < 6; i++)
            {
                if (hold.Contains(i))
                    continue;

                originalDice[i] = _diceRoller.RollDie();
            }

            _currentRoll = new Roll(originalDice);
            return _currentRoll;
        }

        public int Score(int objectiveIndex)
        {
            if(_currentRoll == null)
                throw new InvalidOperationException("Cannot score before dice have been rolled.");

            var score = _objectives[objectiveIndex].Score(_currentRoll);
            _currentRoll = null;
            return score;
        }
    }
}
