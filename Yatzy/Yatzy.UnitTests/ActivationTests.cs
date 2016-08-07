using System;
using FluentAssertions;
using MachineLearning;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yatzy.UnitTests
{
    [TestClass]
    public class ActivationTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            NeuralLayer.ActivationFunction(0.0).Should().Be(0.5);
        }
    }
}
