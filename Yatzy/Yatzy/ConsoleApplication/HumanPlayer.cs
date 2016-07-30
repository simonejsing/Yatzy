using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearning;

namespace ConsoleApplication
{
    class HumanPlayer : IPlayer
    {
        const double Strength = 1.0;
        const double Thresshold = 0.09;

        public HumanPlayer()
        {
        }

        public double[] Respond(IMachineLearningProblem problem)
        {
            // Doing nothing clocks in a score of ~23
            //return new[] {0.0, 0.0};

            var angle = problem.Parameters[0];
            return new[]
            {
                angle > Thresshold ? Strength : 0.0,
                angle < -Thresshold ? Strength : 0.0
            };
        }

    }
}
