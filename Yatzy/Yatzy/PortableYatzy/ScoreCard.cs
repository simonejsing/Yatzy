using System;
using System.Collections.Generic;
using System.Linq;

namespace Yatzy
{
    public class ScoreCard
    {
        private readonly IObjective[] _objectives;
        private int[] _scores;

        public IEnumerable<IObjective> Objectives => _objectives;

        public int[] Scores
        {
            get
            {
                int[] scores = new int[_scores.Length];
                Array.Copy(_scores, scores, _scores.Length);
                return scores;
            }
        }

        public int TotalScore
        {
            get
            {
                return Scores.Sum(s => s > 0 ? s : 0) + PassiveBonusScore();
            }
        }

        public bool Completed {
            get
            {
                return Scores.Where(s => s != -1).Count() == _objectives.Length;
            }
        }

        private int PassiveBonusScore()
        {
            int sum = 0;
            for (int i = 0; i < 6; i++)
            {
                if (_scores[i] == -1)
                    return 0;

                sum += _scores[i];
            }

            return sum >= 84 ? 50 : 0;
        }

        public ScoreCard(ScoreCard scoreCard)
        {
            _objectives = scoreCard.Objectives.ToArray();
            CopyScores(scoreCard);
        }

        public ScoreCard(IObjective[] objectives)
        {
            _objectives = objectives;
            ResetScore();
        }

        private void CopyScores(ScoreCard scoreCard)
        {
            _scores = new int[_objectives.Length];
            for (int i = 0; i < _objectives.Length; i++)
            {
                _scores[i] = scoreCard.Scores[i];
            }
        }

        private void ResetScore()
        {
            _scores = new int[_objectives.Length];
            for (int i = 0; i < _objectives.Length; i++)
            {
                _scores[i] = -1;
            }
        }

        public bool HasObjectiveBeenScored(int objectiveIndex)
        {
            return _scores[objectiveIndex] != -1;
        }

        public int Score(int objectiveIndex, Roll roll)
        {
            if(HasObjectiveBeenScored(objectiveIndex))
                throw new InvalidOperationException("Objective has already been scored.");

            return _scores[objectiveIndex] = _objectives[objectiveIndex].Score(roll);
        }
    }
}