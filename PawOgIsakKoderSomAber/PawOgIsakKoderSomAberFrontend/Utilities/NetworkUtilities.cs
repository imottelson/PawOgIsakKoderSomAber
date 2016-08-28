using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using PawOgIsakKoderSomAberFrontend.Interfaces;
using PawOgIsakKoderSomAberFrontend.Models.NN;
using PawOgIsakKoderSomAberFrontend.Structs;

namespace PawOgIsakKoderSomAberFrontend.Utilities
{
    public class NetworkUtilities : INeuralNetworkUtil
    {
        public Vector Evaluate(Vector input, NeuralNetwork network)
        {
            return ComputeWeightedInputs(input, network).Last();
        }


        public double Sigma(double x)
        {
            return 1.0/(1.0 + Math.Exp(-x));
        }

        public List<Matrix> Grad_w_Regularization(List<Matrix> weightsList)
        {
            return weightsList;
        }


        public double SigmaDiff(double x)
        {
            return Math.Exp(-x)/Math.Pow(Math.Exp(-x) + 1.0, 2);
        }


        public List<Vector> ComputeWeightedInputs(Vector input, NeuralNetwork network)
        {
            int layers = network.WeightsList.Count + 1;

            //The entries in the result list correspond to the weighted input of a given layer
            List<Vector> result = new List<Vector>(layers);
            var x = (Vector)network.WeightsList[0].Multiply(input).Add(network.BiasesList[0]);
            result.Add(x);

            for (int i = 1; i < layers - 1; i++)
            {
                var currentLayer = (Vector)network.WeightsList[i].Multiply(result[i - 1].Map(Sigma)).Add(network.BiasesList[i]);
                result.Add(currentLayer);
            }
            result.Add((Vector)result[layers - 2].Map(Sigma));
            return result;
        }

        //Computes the errors of a network in a given data point for a .

        public List<Vector> ComputeErrors(DataPoint point, NeuralNetwork network, List<Vector> weightedInputs)
        {
            int layers = network.WeightsList.Count+1;

            List<Vector> errors = new List<Vector>(layers-1);
            //TODO: refactor this?
            var currentError = (Vector)weightedInputs[layers - 2].Map(SigmaDiff);

            //Gradient of Cost with respect to output of the neural network:
            var grad = Grad_a_Cost(weightedInputs[layers-1],point.Output);


            //sets error of last layer
            currentError = (Vector)grad.PointwiseMultiply(currentError);
            errors.Add(currentError);

            //succesively sets errors in layers 
            for (int i = 0; i < layers-2; i++)
            {
                var x = (Vector)network.WeightsList[layers - 2 - i].TransposeThisAndMultiply(errors[i]);
                var y = (Vector)weightedInputs[layers - 3 - i].Map(SigmaDiff);
                errors.Add((Vector) x.PointwiseMultiply(y));
            }
            errors.Reverse();
            return errors;
        }

        public List<Matrix> Grad_w_Regularization(NeuralNetwork network)
        {
            return network.WeightsList;
        }

        public double Cost(List<DataPoint> data, NeuralNetwork network)
        {
            double result = 0;
            foreach (var dataPoint in data)
            {
                Vector vec = (Vector)Evaluate(dataPoint.Input, network);
                result += vec.Map(Math.Log).DotProduct(dataPoint.Output)+(1-vec).Map(Math.Log).DotProduct(1-dataPoint.Output);
            }
            return - result / data.Count;
        }

        public Vector Grad_a_Cost(Vector activation,Vector output)
        {
            int length = output.Count;

            var result = new DenseVector(length);

            for (int i = 0; i < length; i++)
            {
                result[i] = -(output[i]/activation[i] - (1 - output[i])/(1 - activation[i]));
            }

            return result;
        }
    }
}
