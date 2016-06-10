using System;
using System.Linq;

namespace NeuralNetworkLibrary
{
    public class Perceptron
    {
        private int inputDimension;
        public int InputDimension { get { return inputDimension; } protected set { inputDimension = value; } }
        private double[] weights;
        public double LearningRate { get; protected set; }
        public Func<double,double> ActivationFunction { get; protected set; }
        
        public Perceptron(int inputDimension, double learningRate, Func<double, double> activationFunction)
        {
            this.inputDimension = inputDimension;
            weights = new double[inputDimension + 1];
            Random randomGenerator = new Random(Guid.NewGuid().GetHashCode());
            for(int i = 0; i < weights.Length; i++)
            {
                weights[i] = randomGenerator.NextDouble() * 2 - 1;
            }
            LearningRate = learningRate;
            ActivationFunction = activationFunction;
        }

        public double Compute(double[] input)
        {
            double output = weights[inputDimension];
            for(int i = 0; i < inputDimension; i++)
            {
                output += weights[i] * input[i];
            }
            return ActivationFunction(output);
        }

        public void Tranning(double[] input, double desiredOutput)
        {
            double output = Compute(input);
            for (int i = 0; i < inputDimension; i++)
            {
                weights[i] += LearningRate * (desiredOutput - output) * input[i];
            }
            weights[inputDimension] += LearningRate * (desiredOutput - output);
        }
    }
}
