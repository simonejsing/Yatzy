using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearning;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var network = new NeuralNetwork(1, 1, 1);

            var outputs = network.Compute(new [] { 0.0 });

            Console.WriteLine("Outputs: {0}", string.Join(",", outputs));
        }
    }
}
