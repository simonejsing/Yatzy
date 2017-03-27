using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MachineLearning;
using Yatzy;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            /*var network = new NeuralNetwork(4, 4, 1, 2);
            network.Compute(new[] { Math.PI / 4 });
            SaveNeuralNetworkAsDGML(network, "network.dgml");*/

            var network = LoadNeuralNetwork(@"..\..\network90.dgml");

            //var player = TrainNeuralNetworkPlayer();
            var player = new NeuralNetworkPlayer(network);
            RunPoleTrainer(player);
            //RunYatzyTrainer();

            AnalyzeNetworkResponse(player.Network, @"..\..\analysis.csv");
        }

        private static void RunPoleTrainer(NeuralNetworkPlayer player)
        {
            const int NumberOfGames = 100;
            const int Iterations = 100;

            double[] scores = new double[NumberOfGames];

            //var player = new DummyPlayer(); // Scores an average of about 11
            //var player = new HumanPlayer(); // Scores an average of about 63

            // Save the network
            var network = new NeuralNetwork(player.Network);
            network.Compute(new[] {Math.PI/4});
            SaveNeuralNetworkAsDGML(network, "network.dgml");

            // Test some examples
            WriteNetworkResponse(network, 0.0);
            WriteNetworkResponse(network, Math.PI / 4);
            WriteNetworkResponse(network, -Math.PI / 4);

            for (int i = 0; i < NumberOfGames; i++)
            {
                var problem = PlayUprightProblem(player, Iterations);
                scores[i] = problem.Score;
                //Console.WriteLine("Final score: {0}", problem.Score);
            }

            double averageScore = scores.Sum() / NumberOfGames;
            double stdDev = Math.Sqrt(scores.Select(s => Sqr(Math.Abs(s - averageScore))).Sum() / NumberOfGames);
            Console.WriteLine("Average score: {0} +/- {1}", averageScore, stdDev);
        }

        private static void WriteNetworkResponse(NeuralNetwork network, double input)
        {
            var input1 = new[] {input};
            var output1 = network.Compute(input1);
            Console.WriteLine("Input: {0}, Output: {1}, {2}", input1[0], output1[0], output1[1]);
        }

        private static void AnalyzeNetworkResponse(NeuralNetwork network, string filePath)
        {
            // Compute the response of the network over the given interval
            var start = -1.0;
            var end = 1.0;
            var delta = 0.01;

            var samples = (end - start)/delta;

            using (var stream = File.Create(filePath))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.WriteLine("Input,Output");
                    for (int i = 0; i <= samples; i++)
                    {
                        var input = start + delta * i;
                        var response = network.Compute(new[] { input });
                        writer.WriteLine("{0},{1}", input, UprightProblem.EvaluatePlayerAction(response));
                    }
                }
            }
        }

        private static NeuralNetwork LoadNeuralNetwork(string filePath)
        {
            DGMLWriter writer = new DGMLWriter();
            var graph = writer.Deserialize(filePath);

            var inputLayerSize = 0;
            var outputLayerSize = 0;
            var numberOfLayers = 0;
            var hiddenLayerSize = 0;

            // Determine depth of neural network
            foreach (var node in graph.Nodes)
            {
                // Parse node structure
                var match = MatchNodeId(node.Id);
                if (!match.Success)
                    continue;

                var layerIndex = int.Parse(match.Groups[1].Value);
                numberOfLayers = Math.Max(numberOfLayers, layerIndex);
            }

            // Determine structure of neural network
            foreach (var node in graph.Nodes)
            {
                // Parse node structure
                var match = MatchNodeId(node.Id);
                if (!match.Success)
                    continue;

                var layerIndex = int.Parse(match.Groups[1].Value);
                var nodeIndex = int.Parse(match.Groups[2].Value);

                if (layerIndex == 0)
                {
                    // Input layer
                    inputLayerSize = Math.Max(inputLayerSize, nodeIndex + 1);
                }
                else if (layerIndex == numberOfLayers)
                {
                    // Output layer
                    outputLayerSize = Math.Max(outputLayerSize, nodeIndex + 1);
                }
                else
                {
                    hiddenLayerSize = Math.Max(hiddenLayerSize, nodeIndex + 1);
                }
            }

            var numberOfHiddenLayers = numberOfLayers - 1;
            var network = new NeuralNetwork(numberOfHiddenLayers, hiddenLayerSize, inputLayerSize, outputLayerSize);

            // Restore network node values
            foreach (var node in graph.Nodes)
            {
                // Parse node structure
                var match = MatchNodeId(node.Id);
                if (!match.Success)
                    continue;

                var layerIndex = int.Parse(match.Groups[1].Value);
                var nodeIndex = int.Parse(match.Groups[2].Value);

                // Parse node value
                var valueMatch = Regex.Match(node.Label, ".+: (.+)");
                if (!valueMatch.Success)
                    throw new Exception(string.Format("Could not parse value for node at [{0},{1}]", layerIndex, nodeIndex));
                var value = double.Parse(valueMatch.Groups[1].Value);

                if (layerIndex == 0)
                {
                    // Input layer
                    network.InputLayer[nodeIndex] = value;
                }
                else if (layerIndex == numberOfLayers)
                {
                    // Output layer
                    network.OutputLayer.Nodes[nodeIndex] = value;
                }
                else
                {
                    network.HiddenLayers[layerIndex-1].Nodes[nodeIndex] = value;
                }
            }

            // Restore network edge values
            foreach (var link in graph.Links)
            {
                var sourceMatch = MatchNodeId(link.Source);
                var targetMatch = MatchNodeId(link.Target);
                if(!sourceMatch.Success || !targetMatch.Success)
                    continue;

                //var sourceLayerIndex = int.Parse(sourceMatch.Groups[1].Value);
                var sourceNodeIndex = int.Parse(sourceMatch.Groups[2].Value);
                var targetLayerIndex = int.Parse(targetMatch.Groups[1].Value);
                var targetNodeIndex = int.Parse(targetMatch.Groups[2].Value);

                // Parse edge value
                var value = double.Parse(link.Label);

                if (targetLayerIndex == 0)
                {
                    throw new Exception("Invalid edge weight targeting input layer.");
                }
                else if (targetLayerIndex == numberOfLayers)
                {
                    network.OutputLayer.InputWeights[targetNodeIndex * hiddenLayerSize + sourceNodeIndex] = value;
                }
                else if (targetLayerIndex == 1)
                {
                    network.HiddenLayers[targetLayerIndex - 1].InputWeights[targetNodeIndex * inputLayerSize + sourceNodeIndex] = value;
                }
                else
                {
                    network.HiddenLayers[targetLayerIndex-1].InputWeights[targetNodeIndex * hiddenLayerSize + sourceNodeIndex] = value;
                }
            }

            return network;
        }

        private static Match MatchNodeId(string input)
        {
            return Regex.Match(input, "L(\\d+)N(\\d+)", RegexOptions.None);
        }

        private static void SaveNeuralNetworkAsDGML(NeuralNetwork network, string path)
        {
            int layerIndex = 0;
            DGMLWriter writer = new DGMLWriter();
            int nodeIndex = 0;
            foreach (var input in network.InputLayer)
            {
                var nodeId = string.Format("L{0}N{1}", 0, nodeIndex);
                var nodeLabel = string.Format("Input: {0:0.0000}", input);
                writer.AddNode(new DGMLWriter.Node(nodeId, nodeLabel, GridBounds(layerIndex, nodeIndex)));
                nodeIndex++;
            }

            foreach (var layer in network.Layers)
            {
                layerIndex++;
                nodeIndex = 0;
                foreach (var node in layer.Nodes)
                {
                    var nodeId = string.Format("L{0}N{1}", layerIndex, nodeIndex);
                    var nodeLabel = string.Format("{0}: {1:0.0000}", nodeId, node);
                    writer.AddNode(new DGMLWriter.Node(nodeId, nodeLabel, GridBounds(layerIndex, nodeIndex)));
                    nodeIndex++;
                }

                var weightIndex = 0;
                foreach (var input in layer.InputWeights)
                {
                    var sourceLayer = layerIndex - 1;
                    var sourceNode = string.Format("L{0}N{1}", sourceLayer, weightIndex % layer.NumberOfInputs);
                    var targetNode = string.Format("L{0}N{1}", layerIndex, weightIndex / layer.NumberOfInputs);
                    var linkLabel = string.Format("{0:0.0000}", input);
                    writer.AddLink(new DGMLWriter.Link(sourceNode, targetNode, linkLabel));
                    weightIndex++;
                }
            }

            writer.Serialize(path);
        }

        private const double GridColumnWidth = 300.0;
        private const double GridRowHeight = 200.0;

        private static string GridBounds(int column, int row)
        {
            double x = column * GridColumnWidth;
            double y = row * GridRowHeight;
            double width = 160.0;
            double height = 25.96;
            return string.Format(@"{0},{1},{2},{3}", x, y, width, height);
        }

        private static NeuralNetworkPlayer TrainNeuralNetworkPlayer()
        {
            const int TrainingIterations = 1000;
            const int NumberOfGames = 100;
            const int Iterations = 100;

            var network = new NeuralNetwork(4, 4, 1, 2);
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
