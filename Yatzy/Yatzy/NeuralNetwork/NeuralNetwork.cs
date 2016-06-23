namespace MachineLearning
{
    public class NeuralNetwork
    {
        private readonly NeuralLayer[] layers;

        public NeuralNetwork(NeuralNetwork network) : this(network.layers.Length)
        {
            for (int i = 0; i < network.layers.Length; i++)
            {
                layers[i] = new NeuralLayer(network.layers[i]);
            }
        }

        public NeuralNetwork(int numberOfLayers)
        {
            layers = new NeuralLayer[numberOfLayers];
            for (int i = 0; i < numberOfLayers; i++)
            {
                layers[i] = new NeuralLayer(2);
            }
        }
    }
}
