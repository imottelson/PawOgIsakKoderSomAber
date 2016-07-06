using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace PawOgIsakKoderSomAber.Interfaces
{
    public interface INeuralNetworkUtil
    {

        double Sigma(double x);

        double SigmaDiff(double x);

        double Cost(List<DataPoint> data, NeuralNetwork network);

        Vector Grad_a_Cost(Vector activation, Vector output);

        List<Vector> ComputeWeightedInputs(Vector input, NeuralNetwork network);

        List<Vector> ComputeErrors(DataPoint point, NeuralNetwork network, List<Vector> weightedInputs );

        List<Matrix> Grad_w_Regularization(NeuralNetwork network);

        Vector Evaluate(Vector input, NeuralNetwork network);
    }
}
