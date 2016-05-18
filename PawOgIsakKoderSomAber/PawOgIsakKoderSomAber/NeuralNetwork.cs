﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using PawOgIsakKoderSomAber.Interfaces;

namespace PawOgIsakKoderSomAber
{
    class NeuralNetwork
    {
        public double StepSize;

        public List<Vector> BiasesList;

        public List<Matrix> WeightsList;

        public NeuralNetworkUtil Utilities { private get; set; }

        public Vector Evaulate(Vector input)
        {
            return Utilities.Evaluate(input, WeightsList, BiasesList);
        }

        public void TrainNetwork(List<DataPoint> batch)
        {
            int batchSize = batch.Count;
            List<List<Vector>> activationsList = new List<List<Vector>>(batchSize);
            List<List<Vector>> errorsList = new List<List<Vector>>(batchSize);

            for (int i = 0; i < batchSize; i++)
            {
                List<Vector> activations = Utilities.EvaluateWithActivationList(batch[i].Input, WeightsList, BiasesList);
                activationsList[i] = activations;
                errorsList[i] = Utilities.ComputeErrors(batch[i], WeightsList, activations);
            }
            List<Vector> errorSum = errorsList[0];
            int errorVectors = errorSum.Count;
            for (int i = 1; i < batchSize; i++)
            {
                for (int j = 0; j < errorVectors; j++)
                {
                    errorSum[j] = (Vector)errorSum[j].Add(errorsList[i][j]);
                }
            }

            int biasesCount = this.BiasesList.Count;
            for (int i = 0; i < biasesCount; i++)
            {
                this.BiasesList[i].MapIndexed((j, value) => { return value - StepSize*errorSum[i][j]/batchSize; });
            }

            int weightsCount = this.WeightsList.Count;
            for (int i = 0; i < weightsCount; i++)
            {
                this.WeightsList[i].MapIndexed((n, m, value) =>
                {
                    double sum = 0;
                    for (int j = 0; j < batchSize; j++)
                    {
                        sum += errorsList[j][n]*activationsList[j][m];
                    }
                    return value - StepSize*sum/batchSize;
                });
            }
        }

        public void TrainNetwork(DataPoint point)
        {
            List<Vector> activations = Utilities.EvaluateWithActivationList(point.Input, WeightsList, BiasesList);
            List<Vector> errors = Utilities.ComputeErrors(point, WeightsList, activations);

            int biasesCount = this.BiasesList.Count;
            for (int i = 0; i < biasesCount; i++)
            {
                this.BiasesList[i].MapIndexed((j, value) => { return value - StepSize*errors[i][j]; });
            }

            int weightsCount = this.WeightsList.Count;
            for (int i = 0; i < weightsCount; i++)
            {
                this.WeightsList[i].MapIndexed((m,n,value) => { return value - StepSize*errors[i][m]*activations[i][n]; });
            }
        }

        private NeuralNetwork()
        {
            this.Utilities = new StandardUtilities();
            this.StepSize = 0.5;
        }

        public NeuralNetwork(List<int> layerSizes) : this()
        {
            int layers = layerSizes.Count();
            List<Vector> biases = new List<Vector>(layers-1);
            List<Matrix> weights = new List<Matrix>(layers-1);
            for (int i = 0; i < layers-1; i++)
            {
                biases[i] = (Vector)Vector.Build.Random(layerSizes[i + 1]);
                weights[i] = (Matrix) Matrix.Build.Random(layerSizes[i], layerSizes[i + 1]);
            }
            this.WeightsList = weights;
            this.BiasesList = biases;
        }
    }
}