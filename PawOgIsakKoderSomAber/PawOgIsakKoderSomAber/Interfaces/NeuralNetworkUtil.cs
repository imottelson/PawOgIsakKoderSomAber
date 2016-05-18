using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace PawOgIsakKoderSomAber.Interfaces
{
    abstract class NeuralNetworkUtil : INeuralNetworkUtil
    {
        public Vector Evaluate(Vector input, List<Matrix> weights, List<Vector> biases)
        {
            return EvaluateWithActivationList(input, weights, biases)[weights.Count + 1];
        }

        public List<Vector> EvaluateWithActivationList(Vector input, List<Matrix> weights, List<Vector> biases)
        {
            int layers = weights.Count + 1;
            List<Vector> result = new List<Vector>(layers);
            input.Map(Sigma, input);

            Vector currentLayer = input;
            for (int i = 0; i < layers; i++)
            {
                currentLayer = (Vector)weights[i].Multiply(currentLayer).Add(biases[i]);
                result[i] = currentLayer;
                currentLayer.Map(Sigma, currentLayer);
            }
            result[layers] = currentLayer;
            return result;
        }
        public abstract double Sigma(double x);

        public abstract double Cost(List<DataPoint> data, List<Matrix> weights, List<Vector> biases);

        public abstract List<Vector> ComputeErrors(DataPoint point, List<Matrix> weights, List<Vector> activations);
    }
}
