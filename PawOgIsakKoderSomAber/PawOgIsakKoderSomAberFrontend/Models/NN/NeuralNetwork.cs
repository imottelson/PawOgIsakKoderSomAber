using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MathNet.Numerics.LinearAlgebra.Double;

namespace PawOgIsakKoderSomAberFrontend.Models.NN
{
    [Serializable]
    public class NeuralNetwork
    {
        public Guid uid;

        [Display(Name = "Step size")]
        public double StepSize { get; set; }

        [Display(Name = "Regularization parameter")]
        public double RegularizationParameter { get; set; }

        //[JsonConverter(typeof(DenseVectorConverter))]
        public List<Vector> BiasesList;

        public List<Matrix> WeightsList;

        public NeuralNetwork()
        {
            uid = new Guid();
        }

        private NeuralNetwork(double stepSize, double regularizationParameter =0)
        {
            uid = Guid.NewGuid();
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