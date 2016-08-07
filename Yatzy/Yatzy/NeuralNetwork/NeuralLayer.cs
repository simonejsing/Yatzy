using System;

namespace MachineLearning
{
    public class NeuralLayer
    {
        private static Random random = new Random();

        public int Size { get; }
        public int NumberOfInputs { get; }
        public double Bias { get; }
        public double[] InputWeights { get; }

        public double[] Nodes { get; }

        public NeuralLayer(int numberOfNeurons, int numberOfInputs, double bias)
        {
            Size = numberOfNeurons;
            NumberOfInputs = numberOfInputs;
            Bias = bias;

            Nodes = new double[Size];
            InputWeights = new double[Size * NumberOfInputs];

            for (int i = 0; i < Size; i++)
            {
                Nodes[i] = 0.0;
            }

            for (int i = 0; i < Size * NumberOfInputs; i++)
            {
                InputWeights[i] = random.NextDouble();
            }
        }

        public NeuralLayer(NeuralLayer layer) : this(layer.Size, layer.NumberOfInputs, layer.Bias)
        {
            Array.Copy(layer.Nodes, Nodes, Size);
            Array.Copy(layer.InputWeights, InputWeights, Size * NumberOfInputs);
        }

        public double[] Compute(double[] inputs)
        {
            UpdateNeuronValues(inputs);
            return Nodes;
        }

        private void UpdateNeuronValues(double[] inputs)
        {
            for (int i = 0; i < Size; i++)
            {
                double weightedInput = Bias;

                for (int j = 0; j < NumberOfInputs; j++)
                {
                    weightedInput += inputs[j]*InputWeights[i*NumberOfInputs + j];
                }

                Nodes[i] = ActivationFunction(weightedInput);
            }
        }

        // Use Sigmoid for now since it is bounded between 0 and 1 which is helpful when we need to make a binary decission
        public static double ActivationFunction(double input)
        {
            return RectifiedLinear(input);
            return Sigmoid(input);
        }

        private static double RectifiedLinear(double input)
        {
            return Math.Max(0.0, input);
        }

        private static double Sigmoid(double input)
        {
            return 1.0/(1.0 + Math.Exp(-input));
        }

        /*private static double DerivedActivationFunction(double input)
        {
            return input < 0.0 ? 0 : 1;
        }*/
    }
}