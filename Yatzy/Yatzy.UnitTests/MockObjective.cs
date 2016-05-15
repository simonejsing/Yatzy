using System;

namespace Yatzy.UnitTests
{
    internal class MockObjective : IObjective
    {
        private readonly Func<Roll, int> _scoringFunc;

        public string Identifier => "Mock";

        public MockObjective(Func<Roll, int> scoringFunc)
        {
            _scoringFunc = scoringFunc;
        }

        public int Score(Roll roll)
        {
            return _scoringFunc(roll);
        }
    }
}