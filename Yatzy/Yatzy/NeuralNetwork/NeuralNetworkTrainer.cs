using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearning
{
    public class NeuralNetworkTrainer
    {
        private NeuralNetwork network;
        private Func<NeuralNetwork, double[], double> scoreFunc;

        public NeuralNetworkTrainer(NeuralNetwork network, Func<NeuralNetwork, double[], double> scoreFunc)
        {
            this.network = network;
            this.scoreFunc = scoreFunc;
        }

        public void Iterate(double[] inputs)
        {
            const double delta = 0.05;
            var score0 = scoreFunc(network, inputs);

            // Compute derivatives with respect to weights in the network
            var totalWeights = network.TotalWeights;
            NeuralNetwork[] permutedNetworks = new NeuralNetwork[totalWeights];
            double[] derivatives = new double[totalWeights];
            double maximum = 0.0;
            int maximizer = 0;

            for (int i = 0; i < totalWeights; i++)
            {
                permutedNetworks[i] = new NeuralNetwork(network);

                // Permute the i'th weight
                PermuteNetworkIndex(permutedNetworks[i], i, delta);

                // Score the permuted network
                var score = scoreFunc(permutedNetworks[i], inputs);
                derivatives[i] = (score - score0) / delta;

                if (Math.Abs(derivatives[i]) > maximum)
                {
                    maximum = Math.Abs(derivatives[i]);
                    maximizer = i;
                }
            }

            // Step along the steepest derivative
            var step = derivatives[maximizer] > 0.0 ? delta : -delta;
            PermuteNetworkIndex(network, maximizer, step);
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