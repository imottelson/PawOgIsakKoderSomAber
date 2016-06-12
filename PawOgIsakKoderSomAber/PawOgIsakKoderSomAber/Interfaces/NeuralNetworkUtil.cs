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
            return EvaluateWithWeightedInputs(input, weights, biases)[weights.Count + 1];
        }

        //Computes the output of the network and the weighted inputs along the way of each layer. The last vector is the output.
        //TODO: maybe takes a network as input? 
        public List<Vector> EvaluateWithWeightedInputs(Vector input, List<Matrix> weights, List<Vector> biases)
        {
            int layers = weights.Count + 1;

            //The entries in the result list correspond to the weighted input of a given layer, first vector is input
            List<Vector> result = new List<Vector>(layers+1);
            result.Add(input);
            
            //Vector currentLayer = input;
            for (int i = 0; i < layers-1; i++)
            {
                var currentLayer = (Vector)weights[i].Multiply(result[i].Map(Sigma)).Add(biases[i]);
                result.Add(currentLayer);
            }
            result.Add((Vector) result[layers-1].Map(Sigma));
            return result;
        }
        public abstract double Sigma(double x);

        public abstract double SigmaDiff(double x);

        public abstract double Cost(List<DataPoint> data, List<Matrix> weights, List<Vector> biases);

        public abstract List<Vector> ComputeErrors(DataPoint point, List<Matrix> weights, List<Vector> activations);
    }
}
