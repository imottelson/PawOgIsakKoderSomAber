using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using MathNet.Numerics.LinearAlgebra.Double;
using PawOgIsakKoderSomAber.Interfaces;

namespace PawOgIsakKoderSomAber
{
    public class NeuralNetwork
    {
        public double StepSize;

        public double RegularizationParameter;

        public List<Vector> BiasesList;

        public List<Matrix> WeightsList;

        private NeuralNetwork(double stepSize, double regularizationParameter =0)
        {
            StepSize = stepSize;
            RegularizationParameter = regularizationParameter;
        }

        public NeuralNetwork(List<int> layerSizes, double stepSize, double regularizationParameter) : this(stepSize, regularizationParameter)
        {
            int layers = layerSizes.Count;
            List<Vector> biases = new List<Vector>(layers-1);
            List<Matrix> weights = new List<Matrix>(layers-1);
            for (int i = 0; i < layers-1; i++)
            {
                biases.Add((Vector)Vector.Build.Random(layerSizes[i+1]));
                weights.Add((Matrix) Matrix.Build.Random(layerSizes[i+1], layerSizes[i]));
            }
            WeightsList = weights;
            BiasesList = biases;
        }
    }
}