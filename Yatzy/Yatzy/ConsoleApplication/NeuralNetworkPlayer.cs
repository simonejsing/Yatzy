using MachineLearning;

namespace ConsoleApplication
{
    internal class NeuralNetworkPlayer : IPlayer
    {
        private readonly NeuralNetwork network;

        public NeuralNetworkPlayer()
        {
            network = new NeuralNetwork(1, 2, 1, 2);
        }

        public double[] Respond(IMachineLearningProblem problem)
        {
            return network.Compute(problem.Parameters);
        }
    }
}