using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearning;

namespace ConsoleApplication
{
    /// <summary>
    /// This problem represents trying to keep an upright pole from falling over.
    /// The score is determined by the number of iterations to the pole falls over... and it will eventually do so.
    /// If the pole starts to drift it will gain momentum till the point where it can fall over.
    /// The pole will be pushed slightly at randomly to simulate instabilities in holding it upright.
    /// There are two inputs, apply left force and apply right force.
    /// If the pole falls more than 90 degrees it is considered fallen over and no more forces apply
    /// </summary>
    class UprightProblem : IMachineLearningProblem
    {
        private const double GravitationalForce = 0.5;
        private const double MaxRandomForce = 0.15;
        private const double MaximumPlayerInputForce = 0.1;

        private static readonly Random random = new Random();

        public double PoleAngle { get; private set; }
        public double Score { get; private set; }

        public double[] Parameters => new [] { PoleAngle };

        public UprightProblem()
        {
            PoleAngle = 0.1;
        }

        public void Update(double[] inputs)
        {
            if (PoleFallenOver(PoleAngle))
                return;

            Score += 1.0;

            var adjustment = 0.0;
            adjustment += RandomForce(PoleAngle);

            // Apply input forces
            adjustment += InputForce(inputs[0], -1);
            adjustment += InputForce(inputs[1], 1);

            // Apply gravity to pole
            adjustment += TangentialForce(PoleAngle, GravitationalForce);

            PoleAngle += adjustment;
        }

        private bool PoleFallenOver(double angle)
        {
            return Math.Abs(angle) > Math.PI/2;
        }

        private static double RandomForce(double angle)
        {
            var magnitude = (random.NextDouble() - 0.5) * MaxRandomForce;
            return Math.Sin(Math.PI / 2 - Math.Abs(angle)) * magnitude;
        }

        private static double TangentialForce(double angle, double force)
        {
            return Math.Sin(angle)*force;
        }

        private double InputForce(double input, double sign)
        {
            var boundedInput = Math.Min(1.0, Math.Abs(input));
            return sign*boundedInput*MaximumPlayerInputForce;
        }
    }
}
