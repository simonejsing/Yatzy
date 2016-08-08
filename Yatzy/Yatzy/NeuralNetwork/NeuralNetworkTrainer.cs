using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearning
{
    public class NeuralNetworkTrainer
    {
        private readonly Func<NeuralNetwork, double[], double> scoreFunc;

        public NeuralNetwork Network { get; private set; }

        public NeuralNetworkTrainer(NeuralNetwork network, Func<NeuralNetwork, double[], double> scoreFunc)
        {
            this.Network = network;
            this.scoreFunc = scoreFunc;
        }

        public double Iterate(double[] inputs)
        {
            const double delta = 0.1;
            var score0 = scoreFunc(Network, inputs);

            // Let's just generate a new random network and pick the best one
            /*var newNetwork = new NeuralNetwork(4, 4, 1, 2);
            var score1 = scoreFunc(newNetwork, inputs);

            if (score1 > score0)
            {
                Network = newNetwork;
            }

            return Math.Max(score0, score1);*/

            // Compute derivatives with respect to weights in the network
            var totalWeights = Network.TotalWeights;
            NeuralNetwork[] permutedNetworks = new NeuralNetwork[totalWeights];
            double[] derivatives = new double[totalWeights];
            double maximum = 0.0;
            double maxScore = score0;
            int maximizer = 0;

            for (int i = 0; i < totalWeights; i++)
            {
                permutedNetworks[i] = new NeuralNetwork(Network);

                // Permute the i'th weight
                PermuteNetworkIndex(permutedNetworks[i], i, delta);

                // Score the permuted network
                var score = scoreFunc(permutedNetworks[i], inputs);
                derivatives[i] = (score - score0) / delta;

                if (Math.Abs(derivatives[i]) > maximum)
                {
                    maxScore = score;
                    maximum = Math.Abs(derivatives[i]);
                    maximizer = i;
                }
            }

            // Step along the steepest derivative
            var step = derivatives[maximizer] > 0.0 ? delta : -delta;
            PermuteNetworkIndex(Network, maximizer, step);
            return maxScore;
        }

        private static void PermuteNetworkIndex(NeuralNetwork network, int i, double delta)
        {
            var neuralLayers = network.Layers.ToArray();

            var index = i;
            for (int j = 0; j < neuralLayers.Length; j++)
            {
                if (index < neuralLayers[j].InputWeights.Length)
                {
                    neuralLayers[j].InputWeights[index] += delta;
                    break;
                }

                index -= neuralLayers[j].InputWeights.Length;
            }
        }
    }
}