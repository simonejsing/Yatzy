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
            RunPoleTrainer();
            //RunYatzyTrainer();
        }

        private static void RunPoleTrainer()
        {
            const int NumberOfGames = 100;
            const int Iterations = 100;

            double[] scores = new double[NumberOfGames];

            //var player = new DummyPlayer(); // Scores an average of about 11
            //var player = new HumanPlayer(); // Scores an average of about 63
            var player = TrainNeuralNetworkPlayer();

            // Save the network
            SaveNeuralNetworkAsDGML(player.Network, "network.dgml");

            for (int i = 0; i < NumberOfGames; i++)
            {
                var problem = PlayUprightProblem(player, Iterations);
                scores[i] = problem.Score;
                Console.WriteLine("Final score: {0}", problem.Score);
            }

            double averageScore = scores.Sum() / NumberOfGames;
            double stdDev = Math.Sqrt(scores.Select(s => Sqr(Math.Abs(s - averageScore))).Sum() / NumberOfGames);
            Console.WriteLine("Average score: {0} +/- {1}", averageScore, stdDev);
        }

        private static void SaveNeuralNetworkAsDGML(NeuralNetwork network, string path)
        {
            int layerIndex = 0;
            DGMLWriter writer = new DGMLWriter();
            int nodeIndex = 0;
            foreach (var input in network.InputLayer)
            {
                var nodeId = string.Format("L{0}N{1}", 0, nodeIndex);
                var nodeLabel = string.Format("{0}: {1}", nodeId, input);
                writer.AddNode(new DGMLWriter.Node(nodeId, nodeLabel));
                nodeIndex++;
            }

            foreach (var layer in network.Layers)
            {
                layerIndex++;
                nodeIndex = 0;
                foreach (var node in layer.Nodes)
                {
                    var nodeId = string.Format("L{0}N{1}", layerIndex, nodeIndex);
                    var nodeLabel = string.Format("{0}: {1}", nodeId, node);
                    writer.AddNode(new DGMLWriter.Node(nodeId, nodeLabel));
                    nodeIndex++;
                }

                var weightIndex = 0;
                foreach (var input in layer.InputWeights)
                {
                    var sourceLayer = layerIndex - 1;
                    var sourceNode = string.Format("L{0}N{1}", sourceLayer, weightIndex % layer.NumberOfInputs);
                    var targetNode = string.Format("L{0}N{1}", layerIndex, weightIndex / layer.NumberOfInputs);
                    var linkLabel = string.Format("{0}", input);
                    writer.AddLink(new DGMLWriter.Link(sourceNode, targetNode, linkLabel));
                    weightIndex++;
                }
            }

            writer.Serialize(path);
        }

        private static NeuralNetworkPlayer TrainNeuralNetworkPlayer()
        {
            const int TrainingIterations = 100;
            const int NumberOfGames = 100;
            const int Iterations = 20;

            var network = new NeuralNetwork(2, 2, 1, 2);
            var trainer = new NeuralNetworkTrainer(network, (newNetwork, inputs) =>
            {
                var player = new NeuralNetworkPlayer(newNetwork);
                var sumScore = 0.0;
                for (int i = 0; i < NumberOfGames; i++)
                {
                    var problem = PlayUprightProblem(player, Iterations);
                    sumScore += problem.Score;
                }

                return sumScore/NumberOfGames;
            });

            // Train the neural network
            for (int j = 0; j < TrainingIterations; j++)
            {
                var bestScore = trainer.Iterate(new double[0]);
                Console.WriteLine("Current score: {0}", bestScore);
            }
            return new NeuralNetworkPlayer(trainer.Network);
        }

        private static double Sqr(double value)
        {
            return value * value;
        }

        private static UprightProblem PlayUprightProblem(IPlayer player, int iterations)
        {
            var problem = new UprightProblem();

            for (int i = 0; i < iterations; i++)
            {
                //Console.WriteLine("Angle: {0}", problem.PoleAngle);
                problem.Update(player.Respond(problem));
            }

            return problem;
        }

        /*private static void RunYatzyTrainer()
        {
            var dieRoller = new RandomDieRoller();
            var network = new NeuralNetwork(2, 3, 1, 1);

            // Train network
            const int Iterations = 10000;
            var trainer = new NeuralNetworkTrainer(network, ScoreFunc);

            for (int i = 0; i < Iterations; i++)
            {
                var inputs = new[] {NormalizeDieValue(dieRoller.RollDie())};
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
        }*/
    }
}
