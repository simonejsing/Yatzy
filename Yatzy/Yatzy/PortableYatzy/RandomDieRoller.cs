using System;

namespace Yatzy
{
    internal class RandomDieRoller : IDiceRoller
    {
        private const int Seed = 1;
        private const int BufferSize = 100;

        private readonly Random _random;
        private readonly byte[] _buffer;
        private int _index = 0;

        public RandomDieRoller()
        {
            _random = new Random();
            //_random = new Random(Seed);
            _buffer = new byte[BufferSize];

            ResetBuffer();
        }

        private void ResetBuffer()
        {
            _random.NextBytes(_buffer);
            _index = 0;
        }

        public byte RollDie()
        {
            byte randomNumber;

            // Keep rolling and discard any rolls equal to or above 42*6 since we can't uniformly convert those to 1-6
            do
            {
                randomNumber = InternalRollDie();
            } while (randomNumber >= 42*6);

            // Convert roll to 1-6
            return Convert.ToByte(1 + randomNumber % 6);
        }

        private byte InternalRollDie()
        {
            if (_index >= _buffer.Length)
                ResetBuffer();

            return _buffer[_index++];
        }
    }
}