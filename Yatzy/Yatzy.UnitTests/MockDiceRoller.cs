using System.Collections;
using System.Linq;

namespace Yatzy.UnitTests
{
    internal class MockDiceRoller : IDiceRoller
    {
        private readonly byte[] _dieRolls;
        private int _index = 0;

        public MockDiceRoller(byte[] dieRolls)
        {
            _dieRolls = dieRolls;
        }

        public byte RollDie()
        {
            if (_index >= _dieRolls.Length)
                _index = 0;

            return _dieRolls[_index++];
        }
    }
}