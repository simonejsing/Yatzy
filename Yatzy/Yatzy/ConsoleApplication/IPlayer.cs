using MachineLearning;

namespace ConsoleApplication
{
    internal interface IPlayer
    {
        double[] Respond(IMachineLearningProblem problem);
    }
}