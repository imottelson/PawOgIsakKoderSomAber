using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using PawOgIsakKoderSomAber.Interfaces;

namespace PawOgIsakKoderSomAber
{
    class StandardUtilities : INeuralNetworkUtil
    {
        public Vector Evaluate(Vector input, List<Matrix> weights, List<Vector> biases)
        {
            throw new NotImplementedException();
        }

        public double Sigma(double x)
        {
            return 1.0/(1.0 + Math.Exp(-x));
        }

        public double Cost(List<DataPoint> data, List<Matrix> weights, List<Vector> biases)
        {
            double result = 0;
            foreach (var dataPoint in data)
            {
                Vector vec = (Vector)Evaluate(dataPoint.Input, weights, biases).Subtract(dataPoint.Output);
                result += vec.DotProduct(vec);
            }
            return result/data.Count;
        }


    }
}
