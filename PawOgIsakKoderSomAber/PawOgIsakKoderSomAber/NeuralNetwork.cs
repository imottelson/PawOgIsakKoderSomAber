using System;
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

        //Computes the output of the neural network given the input vector
        public Vector Evaulate(Vector input)
        {
            return Utilities.Evaluate(input, WeightsList, BiasesList);
        }

        //Updates the weights and biases for the network based on the gradient of the cost function of a given batch
        public void TrainNetwork(List<DataPoint> batch)
        {
            int batchSize = batch.Count;

            //Each entry contains all the weighted inputs ("z's") of the network corresponding to a given data point
            List<List<Vector>> weightedInputsList = new List<List<Vector>>(batchSize);

            //Each entry contains all the errors of the netork corresponding to a given data point
            List<List<Vector>> errorsList = new List<List<Vector>>(batchSize);

            //populate the weightedInputsList and errorsList:
            for (int i = 0; i < batchSize; i++)
            {
                //Gets the weighted inputs for each layer:
                List<Vector> weightedInputs = Utilities.EvaluateWithWeightedInputs(batch[i].Input, WeightsList, BiasesList);
                //Set the weighted inputs
                weightedInputsList.Add(weightedInputs);
                //Set the errors
                errorsList.Add(Utilities.ComputeErrors(batch[i], WeightsList, weightedInputs));
            }
            List<Vector> errorSum = errorsList[0];
            int errorVectors = errorSum.Count;
            //The first errors are already included, so we start at 1:
            for (int i = 1; i < batchSize; i++)
            {
                for (int j = 0; j < errorVectors; j++)
                {
                    errorSum[j]=(Vector)errorSum[j].Add(errorsList[i][j]);
                }
            }

            int biasesCount = BiasesList.Count;
            for (int i = 0; i < biasesCount; i++)
            {
                BiasesList[i] = (Vector)BiasesList[i].MapIndexed((j, value) => { return value - StepSize*errorSum[i][j]/batchSize; });
            }

            int weightsCount = WeightsList.Count;
            for (int i = 0; i < weightsCount; i++)
            {
                WeightsList[i] = (Matrix)WeightsList[i].MapIndexed((n, m, value) =>
                {
                    double sum = 0;
                    for (int j = 0; j < batchSize; j++)
                    {
                        var activation = Utilities.Sigma(weightedInputsList[j][i][m]);
                        var error = errorsList[j][i][n];
                        sum += error*activation;
                    }
                    return value - StepSize*sum/batchSize;
                });
            }
        }

        //not needed?
        public void TrainNetwork(DataPoint point)
        {
            List<Vector> activations = Utilities.EvaluateWithWeightedInputs(point.Input, WeightsList, BiasesList);
            List<Vector> errors = Utilities.ComputeErrors(point, WeightsList, activations);

            int biasesCount = BiasesList.Count;
            for (int i = 0; i < biasesCount; i++)
            {
                BiasesList[i].MapIndexed((j, value) => { return value - StepSize*errors[i][j]; });
            }

            int weightsCount = WeightsList.Count;
            for (int i = 0; i < weightsCount; i++)
            {
                WeightsList[i].MapIndexed((m,n,value) => { return value - StepSize*errors[i][m]*activations[i+1][n]; });
            }
        }

        private NeuralNetwork(double stepSize)
        {
            Utilities = new StandardUtilities();
            StepSize = stepSize;
        }

        public NeuralNetwork(List<int> layerSizes, double stepSize) : this(stepSize)
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