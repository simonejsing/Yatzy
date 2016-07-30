using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public interface IMachineLearningProblem
    {
        double[] Parameters { get; }

        void Update(double[] inputs);
    }
}
