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
        //TODO: work out how to apply functions to vectors correctly!
        public override double Sigma(double x)
        {
            return 1.0/(1.0 + Math.Exp(-x));
        }

        public Vector SigmaVector(Vector x)
        {
            var output = new DenseVector(x.Count);
            for(int i =0;i<x.Count;i++)
            {
                output[i] = Sigma(x[i]);
            }
            return output;
        }
        public override double SigmaDiff(double x)
        {
            return Math.Exp(-x)/Math.Pow(Math.Exp(-x) + 1.0, 2);
        }

        public Vector SigmaDiffVector(Vector x)
        {
            var output = new DenseVector(x.Count);
            for (int i = 0; i < x.Count; i++)
            {
                output[i] = SigmaDiff(x[i]);
            }
            return output;
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
            var currentError = (Vector)weightedInputs[layers - 2].Map(SigmaDiff);

            //Gradient of Cost with respect to output of the neural network:
            var grad = (Vector)weightedInputs[layers-1].Subtract(point.Output);

            //sets error of last layer
            currentError = (Vector)grad.PointwiseMultiply(currentError);
            errors.Add(currentError);

            //succesively sets errors in layers 
            for (int i = 0; i < layers-2; i++)
            {
                var x = (Vector)weights[layers - 2 - i].TransposeThisAndMultiply(errors[i]);
                var y = (Vector)weightedInputs[layers - 3 - i].Map(SigmaDiff);
                errors.Add((Vector) x.PointwiseMultiply(y));
            }
            errors.Reverse();
            return errors;
        }  
    }
}
