using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;
using PawOgIsakKoderSomAberFrontend.Interfaces;
using PawOgIsakKoderSomAberFrontend.Models.NN;
using PawOgIsakKoderSomAberFrontend.Structs;

namespace PawOgIsakKoderSomAberFrontend.Helpers
{
    public class NetworkHelper
    {
        public INeuralNetworkUtil NetworkUtilities;

        public NetworkHelper(INeuralNetworkUtil networkUtilities)
        {
            NetworkUtilities = networkUtilities;
        }

        public Vector Evaluate(Vector input, NeuralNetwork network)
        {
            return NetworkUtilities.Evaluate(input,network);
        }

        public void TrainNetwork(NeuralNetwork network, List<DataPoint> batch )
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
                List<Vector> weightedInputs = NetworkUtilities.ComputeWeightedInputs(batch[i].Input, network);
                //Set the weighted inputs
                weightedInputsList.Add(weightedInputs);
                //Set the errors
                errorsList.Add(NetworkUtilities.ComputeErrors(batch[i], network, weightedInputs));
            }
            List<Vector> errorSum = errorsList[0];
            int errorVectors = errorSum.Count;
            //The first errors are already included, so we start at 1:
            for (int i = 1; i < batchSize; i++)
            {
                for (int j = 0; j < errorVectors; j++)
                {
                    errorSum[j] = (Vector)errorSum[j].Add(errorsList[i][j]);
                }
            }

            int biasesCount = network.BiasesList.Count;
            for (int i = 0; i < biasesCount; i++)
            {
                Vector delta_b = errorSum[i];
                network.BiasesList[i] = (Vector)(network.BiasesList[i] - delta_b * network.StepSize / batchSize);
            }

            int weightsCount = network.WeightsList.Count;

            //Modify the first weight, using the inputs:
            var delta_w0 = errorsList[0][0].OuterProduct(batch[0].Input);
            for (int i = 1; i < batchSize; i++)
            {
                delta_w0 = delta_w0 + errorsList[i][0].OuterProduct(batch[i].Input);
            }
            network.WeightsList[0] = (Matrix)(network.WeightsList[0] - network.StepSize / batchSize * delta_w0 - network.RegularizationParameter * NetworkUtilities.Grad_w_Regularization(network)[0]);

            //Modify the rest of the weights:
            for (int l = 1; l < weightsCount; l++)
            {
                var delta_w = errorsList[0][l].OuterProduct(weightedInputsList[0][l - 1].Map(NetworkUtilities.Sigma));
                for (int i = 1; i < batchSize; i++)
                {
                    delta_w =
                        delta_w + errorsList[i][l].OuterProduct(weightedInputsList[i][l - 1].Map(NetworkUtilities.Sigma));
                }
                network.WeightsList[l] = (Matrix)(network.WeightsList[l] - network.StepSize / batchSize * delta_w - network.RegularizationParameter * NetworkUtilities.Grad_w_Regularization(network)[l]);
            }
        }
    }
}
