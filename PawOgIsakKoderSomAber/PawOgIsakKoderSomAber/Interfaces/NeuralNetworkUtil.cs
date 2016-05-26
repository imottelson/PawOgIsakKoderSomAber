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

        //Computes the output of the network given a list of activations, as well as the activations of each layer. The first vector of activations is the input and the last vecotr is the output.
        //TODO: maybe takes a network as input? 
        public List<Vector> EvaluateWithActivationList(Vector input, List<Matrix> weights, List<Vector> biases)
        {
            int layers = weights.Count + 1;
            //The entries in the result list correspond to the activations of a given layer
            List<Vector> result = new List<Vector>(layers);
            result[0] = input;
            
            Vector currentLayer = input;
            for (int i = 0; i < layers; i++)
            {
                currentLayer = (Vector)weights[i].Multiply(currentLayer).Add(biases[i]);
                currentLayer.Map(Sigma, currentLayer);
                result[i+1] = currentLayer;
            }
            return result;
        }
        public abstract double Sigma(double x);

        public abstract double Cost(List<DataPoint> data, List<Matrix> weights, List<Vector> biases);

        public abstract List<Vector> ComputeErrors(DataPoint point, List<Matrix> weights, List<Vector> activations);
    }
}
