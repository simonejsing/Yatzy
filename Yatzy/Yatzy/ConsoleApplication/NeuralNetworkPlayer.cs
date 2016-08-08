using System;
using MachineLearning;

namespace ConsoleApplication
{
    internal class NeuralNetworkPlayer : IPlayer
    {
        public NeuralNetwork Network { get; }

        public NeuralNetworkPlayer(NeuralNetwork network)
        {
            Network = network;
        }

        public double[] Respond(IMachineLearningProblem problem)
        {
            double[] inputs = new double[1];
            //inputs[0] = NormalizeParameter(problem.Parameters[0]);
            inputs[0] = problem.Parameters[0];

            double[] response = Network.Compute(inputs);

            if (response[0] > response[1])
            {
                response[1] = 0.0;
            }
            else
            {
                response[0] = 0.0;
            }

            return response;
        }

        private static double NormalizeParameter(double parameter)
        {
            // This value is the angle of the pole, it goes from -PI/2 to PI/2
            // we need to map this to [0,1]
            var normalizedParameter = (parameter + Math.PI/2) / Math.PI;

            return BoundParameter(normalizedParameter);
        }

        private static double BoundParameter(double normalizedParameter)
        {
            return Math.Min(1.0, Math.Abs(normalizedParameter));
        }
    }
} 