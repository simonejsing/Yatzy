using System;

namespace MachineLearning
{
    internal class NeuralLayer
    {
        public int Size { get; }
        public int NumberOfInputs { get; }

        private readonly double bias;
        private readonly double[] nodes;
        private readonly double[] inputWeights;

        public NeuralLayer(int numberOfNeurons, int numberOfInputs)
        {
            Size = numberOfNeurons;
            NumberOfInputs = numberOfInputs;

            bias = 0.0;
            nodes = new double[Size];
            inputWeights = new double[Size * NumberOfInputs];

            for (int i = 0; i < Size; i++)
            {
                nodes[i] = 0.0;
            }

            for (int i = 0; i < Size * NumberOfInputs; i++)
            {
                inputWeights[i] = 0.0;
            }
        }

        public NeuralLayer(NeuralLayer layer) : this(layer.Size, layer.NumberOfInputs)
        {
            bias = layer.bias;

            Array.Copy(layer.nodes, nodes, Size);
            Array.Copy(layer.inputWeights, inputWeights, Size * NumberOfInputs);
        }

        public double[] Compute(double[] inputs)
        {
            UpdateNeuronValues(inputs);
            return nodes;
        }

        private void UpdateNeuronValues(double[] inputs)
        {
            for (int i = 0; i < Size; i++)
            {
                double weightedInput = bias;

                for (int j = 0; j < NumberOfInputs; j++)
                {
                    weightedInput += inputs[j]*inputWeights[i*NumberOfInputs + j];
                }

                nodes[i] = ActivationFunction(weightedInput);
            }
        }

        private static double ActivationFunction(double input)
        {
            return Math.Max(0.0, input);
        }

        private static double DerivedActivationFunction(double input)
        {
            return input < 0.0 ? 0 : 1;
        }
    }
}