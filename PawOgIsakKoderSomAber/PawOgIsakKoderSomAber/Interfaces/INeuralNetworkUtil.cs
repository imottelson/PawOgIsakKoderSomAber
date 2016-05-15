using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace PawOgIsakKoderSomAber.Interfaces
{
    interface INeuralNetworkUtil
    {
        Vector Evaluate(Vector input, List<Matrix> weights, List<Vector> biases);

        double Sigma(double x);

        double Cost(List<DataPoint> data, List<Matrix> weights, List<Vector> biases);
    }
}
