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
            return ComputeWeightedInputs(input, weights, biases)[weights.Count];
        }

        //Computes the output of the network and the weighted inputs along the way of each layer. The last vector is the output.
        //TODO: maybe takes a network as input? 
        public List<Vector> ComputeWeightedInputs(Vector input, List<Matrix> weights, List<Vector> biases)
        {
            int layers = weights.Count + 1;

            //The entries in the result list correspond to the weighted input of a given layer
            List<Vector> result = new List<Vector>(layers);
            var x = (Vector) weights[0].Multiply(input).Add(biases[0]);
            result.Add(x);
            
            for (int i = 1; i < layers-1; i++)
            {
                var currentLayer = (Vector)weights[i].Multiply(result[i-1].Map(Sigma)).Add(biases[i]);
                result.Add(currentLayer);
            }
            result.Add((Vector) result[layers-2].Map(Sigma));
            return result;
        }
        public abstract double Sigma(double x);

        public abstract double SigmaDiff(double x);

        public abstract double Cost(List<DataPoint> data, List<Matrix> weights, List<Vector> biases);

        public abstract List<Vector> ComputeErrors(DataPoint point, List<Matrix> weights, List<Vector> activations);
    }
}
