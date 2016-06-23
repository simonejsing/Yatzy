using System;

namespace MachineLearning
{
    public class NeuralNetwork
    {
        private int NumberOfHiddenLayers { get; }
        private int NumberOfInputs { get; }
        private int NumberOfOutputs { get; }

        private readonly NeuralLayer[] hiddenLayers;
        private readonly NeuralLayer outputLayer;

        public NeuralNetwork(NeuralNetwork network) : this(network.NumberOfHiddenLayers, network.NumberOfInputs, network.NumberOfOutputs)
        {
            for (int i = 0; i < NumberOfHiddenLayers; i++)
            {
                hiddenLayers[i] = new NeuralLayer(network.hiddenLayers[i]);
            }

            outputLayer = new NeuralLayer(network.outputLayer);
        }

        public NeuralNetwork(int numberOfLayers, int numberOfInputs, int numberOfOutputs)
        {
            const int hiddenLayerSize = 2;
            const double hiddenLayerBias = 0.0;

            NumberOfHiddenLayers = numberOfLayers;
            NumberOfInputs = numberOfInputs;
            NumberOfOutputs = numberOfOutputs;

            hiddenLayers = new NeuralLayer[numberOfLayers];
            for (int i = 0; i < numberOfLayers; i++)
            {
                hiddenLayers[i] = new NeuralLayer(hiddenLayerSize, numberOfInputs, hiddenLayerBias);
            }

            outputLayer = new NeuralLayer(numberOfOutputs, hiddenLayerSize, 0.0);
        }

        public double[] Compute(double[] inputs)
        {
            if (inputs.Length != NumberOfInputs)
            {
                throw new InvalidOperationException("Incorrect number of inputs provided.");
            }

            var currentInputs = inputs;

            for (int i = 0; i < NumberOfHiddenLayers; i++)
            {
                currentInputs = hiddenLayers[i].Compute(currentInputs);
            }

            return outputLayer.Compute(currentInputs);
        }
    }
}
