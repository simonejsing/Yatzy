using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearning
{
    public class NeuralNetwork
    {
        public int NumberOfHiddenLayers { get; }
        public int NumberOfInputs { get; }
        public int NumberOfOutputs { get; }
        public int TotalWeights => Layers.Sum(l => l.InputWeights.Length);

        public double[] InputLayer { get; }
        public NeuralLayer[] HiddenLayers { get; }
        public NeuralLayer OutputLayer { get; }
        public IEnumerable<NeuralLayer> Layers => HiddenLayers.Concat(new [] { OutputLayer });

        public NeuralNetwork(NeuralNetwork network) : this(network.NumberOfHiddenLayers, 1, network.NumberOfInputs, network.NumberOfOutputs)
        {
            for (int i = 0; i < NumberOfHiddenLayers; i++)
            {
                HiddenLayers[i] = new NeuralLayer(network.HiddenLayers[i]);
            }

            InputLayer = new double[network.NumberOfInputs];
            for (int i = 0; i < network.NumberOfInputs; i++)
            {
                InputLayer[i] = network.InputLayer[i];
            }
            OutputLayer = new NeuralLayer(network.OutputLayer);
        }

        public NeuralNetwork(int numberOfLayers, int hiddenLayerSize, int numberOfInputs, int numberOfOutputs)
        {
            const double hiddenLayerBias = 0.0;
            //const double hiddenLayerBias = 0.5;

            NumberOfHiddenLayers = numberOfLayers;
            NumberOfInputs = numberOfInputs;
            NumberOfOutputs = numberOfOutputs;

            HiddenLayers = new NeuralLayer[numberOfLayers];
            for (int i = 0; i < numberOfLayers; i++)
            {
                // First layer has number of inputs equal to number of inputs to the network, consecutive layers will inherit from previous layer's size
                var layerInputs = i == 0 ? numberOfInputs : HiddenLayers[i-1].Size;
                HiddenLayers[i] = new NeuralLayer(hiddenLayerSize, layerInputs, hiddenLayerBias);
            }

            InputLayer = new double[numberOfInputs];
            for (int i = 0; i < numberOfInputs; i++)
            {
                InputLayer[i] = 0.0;
            }
            OutputLayer = new NeuralLayer(numberOfOutputs, hiddenLayerSize, 0.0);
        }

        public double[] Compute(double[] inputs)
        {
            if (inputs.Length != NumberOfInputs)
            {
                throw new InvalidOperationException("Incorrect number of inputs provided.");
            }

            for (int i = 0; i < NumberOfInputs; i++)
            {
                InputLayer[i] = inputs[i];
            }

            var currentInputs = inputs;

            for (int i = 0; i < NumberOfHiddenLayers; i++)
            {
                currentInputs = HiddenLayers[i].Compute(currentInputs);
            }

            return OutputLayer.Compute(currentInputs);
        }
    }
}
