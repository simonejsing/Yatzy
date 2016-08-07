using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearning;

namespace ConsoleApplication
{
    class DummyPlayer : IPlayer
    {
        public double[] Respond(IMachineLearningProblem problem)
        {
            return new [] { 0.0, 0.0 };
        }
    }
}
