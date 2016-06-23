using System;

namespace MachineLearning
{
    internal class NeuralLayer
    {
        public int Size { get; private set; }

        private double[] nodes;
        private double[] inputWeights;
        private double[] outputWeights;

        public NeuralLayer(int numberOfNeurons)
        {
            Size = numberOfNeurons;

            nodes = new double[Size];
            inputWeights = new double[Size];
            outputWeights = new double[Size];

            for (int i = 0; i < Size; i++)
            {
                nodes[i] = 0.0;
                inputWeights[i] = 0.0;
                outputWeights[i] = 0.0;
            }
        }

        public NeuralLayer(NeuralLayer layer) : this(layer.Size)
        {
            for (int i = 0; i < Size; i++)
            {
                nodes[i] = layer.nodes[i];
                inputWeights[i] = layer.inputWeights[i];
                outputWeights[i] = layer.outputWeights[i];
            }
        }

        private static double ActivationFunction(double input)
        {
            return Math.Max(0.5, input);
        }

        private static double DerivedActivationFunction(double input)
        {
            return input < 0.5 ? 0 : 1;
        }
    }
}