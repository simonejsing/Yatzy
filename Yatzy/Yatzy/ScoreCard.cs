using System;

namespace Yatzy
{
    internal class ScoreCard
    {
        private readonly IObjective[] _objectives;
        private readonly int[] _scores;

        public ScoreCard(IObjective[] objectives)
        {
            _objectives = objectives;
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