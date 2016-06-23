using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearning;
using Yatzy;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var dieRoller = new RandomDieRoller();
            var network = new NeuralNetwork(2, 3, 1, 1);

            // Train network
            const int Iterations = 10000;
            var trainer = new NeuralNetworkTrainer(network, ScoreFunc);

            for (int i = 0; i < Iterations; i++)
            {
                var inputs = new[] { NormalizeDieValue(dieRoller.RollDie()) };
                //var inputs = new[] { 0.0 };

                //var score0 = ScoreFunc(network, inputs);
                trainer.Iterate(inputs);
                //var scoreN = ScoreFunc(network, inputs);

                //Console.WriteLine("Scores: {0} {1}", score0, scoreN);
            }

            // Test network
            for (byte i = 1; i <= 6; i++)
            {
                TestNetwork(network, i);
            }
        }

        private static double ScoreFunc(NeuralNetwork neuralNetwork, double[] inputs)
        {
            var denormalizedInput = inputs[0] * 5.0 + 1.0;
            var outputs = neuralNetwork.Compute(inputs);

            if (outputs[0] > 0.5)
            {
                return denormalizedInput;
            }

            // 3.5 is the expected value of repeatedly rolling a die
            return 3.5 - denormalizedInput;
        }

        private static void TestNetwork(NeuralNetwork network, byte die)
        {
            // Normalize the die value
            var inputs = new double[] {NormalizeDieValue(die)};

            // Compute the networks output
            var outputs = network.Compute(inputs);
            var action = outputs[0] > 0.5 ? "Hold" : "Reroll";
            var score = ScoreFunc(network, inputs);

            Console.WriteLine("Inputs: {0:0.0} - Outputs: {1} - Action: {2} - Score: {3}", inputs[0], outputs[0], action, score);
        }

        private static double NormalizeDieValue(byte die)
        {
            return (die - 1.0)/5.0;
        }
    }
}
