using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using PawOgIsakKoderSomAber.Interfaces;

namespace PawOgIsakKoderSomAber
{
    class StandardUtilities : NeuralNetworkUtil
    {
        public override double Sigma(double x)
        {
            return 1.0/(1.0 + Math.Exp(-x));
        }

        private double SigmaDiff(double x)
        {
            return Math.Exp(x)/Math.Pow(Math.Exp(x) + 1, 2);
        }

        public override double Cost(List<DataPoint> data, List<Matrix> weights, List<Vector> biases)
        {
            double result = 0;
            foreach (var dataPoint in data)
            {
                Vector vec = (Vector)Evaluate(dataPoint.Input, weights, biases).Subtract(dataPoint.Output);
                result += vec.DotProduct(vec);
            }
            return result/data.Count;
        }
    
        //Computes the errors of a network in a given data point for a .
        //TODO: maybe takes a network as input?
        public override List<Vector> ComputeErrors(DataPoint point, List<Matrix> weights, List<Vector> weightedInputs)
        {
            int layers = weights.Count+1;

            List<Vector> errors = new List<Vector>(layers-1);
            //TODO: refactor this?
            //set currentError equal to the input to the output layer
            var currentError = weightedInputs[layers - 1];
            currentError.Map(SigmaDiff, currentError);

            //Gradient of Cost with respect to output of the neural network:
            var grad = (Vector) point.Output.Subtract(weightedInputs[layers]);

            //sets error of last layer
            currentError = (Vector) grad.PointwiseMultiply(currentError);
            errors.Insert(0, currentError);

            //succesively sets errors in layers 
            for (int i = 0; i < layers-2; i++)
            {
                currentError = (Vector)weights[layers-2-i].TransposeThisAndMultiply(currentError);
                var applySigmaDiff = new DenseVector(weightedInputs[layers-2-i].Count);
                weightedInputs[layers - 2 - i].Map(SigmaDiff, applySigmaDiff);
                currentError = (Vector) currentError.PointwiseMultiply(applySigmaDiff);
                errors.Insert(i+1, currentError);
            }
            errors.Reverse();
            return errors;
        }  
    }
}
