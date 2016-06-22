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
    class NeuralNetwork
    {
        public double StepSize;

        public List<Vector> BiasesList;

        public List<Matrix> WeightsList;

        public NeuralNetworkUtil Utilities { get; set; }

        //Computes the output of the neural network given the input vector
        public int Evaulate(Vector input)
        {
            return Utilities.Evaluate(input, WeightsList, BiasesList).AbsoluteMaximumIndex();
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
                List<Vector> weightedInputs = Utilities.ComputeWeightedInputs(batch[i].Input, WeightsList, BiasesList);
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
                Vector delta_b = (Vector) errorSum[i];
                BiasesList[i] = (Vector) BiasesList[i].Subtract(delta_b.Multiply(StepSize/batchSize));
            }

            int weightsCount = WeightsList.Count;

            //Modify the first weight, using the inputs:
            var delta_w0 = errorsList[0][0].OuterProduct(batch[0].Input);
            for (int i = 1; i < batchSize; i++)
            {
                delta_w0 = delta_w0.Add(errorsList[i][0].OuterProduct(batch[i].Input));
            }
            WeightsList[0] = (Matrix)WeightsList[0].Subtract(delta_w0.Multiply(StepSize / batchSize));

            //Modify the rest of the weights:
            for (int l = 1; l < weightsCount;l++)
            {
                var delta_w = errorsList[0][l].OuterProduct(weightedInputsList[0][l-1].Map(Utilities.Sigma));
                for (int i = 1; i < batchSize; i++)
                {
                    delta_w =
                        delta_w.Add(errorsList[i][l].OuterProduct(weightedInputsList[i][l - 1].Map(Utilities.Sigma)));
                }
                WeightsList[l] = (Matrix) WeightsList[l].Subtract(delta_w.Multiply(StepSize/batchSize));
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