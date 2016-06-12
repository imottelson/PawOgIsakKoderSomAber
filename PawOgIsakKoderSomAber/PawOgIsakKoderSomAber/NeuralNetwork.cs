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
                //var x = BiasesList[i];
                //var y = (Vector)BiasesList[i].MapIndexed((j, value) => value - StepSize*errorSum[i][j]/batchSize);
                //BiasesList[i] = y;
                Vector delta_b = (Vector) errorSum[i];
                BiasesList[i] = (Vector) BiasesList[i].Subtract(delta_b.Multiply(StepSize/batchSize));
            }

            int weightsCount = WeightsList.Count;
            for (int i = 0; i < weightsCount; i++)
            {
                Matrix delta_w =(Matrix) errorsList[0][i].OuterProduct(weightedInputsList[0][i].Map(Utilities.Sigma));
                for (int j = 1; j < batchSize; j++)
                {
                    delta_w =
                        (Matrix)
                            delta_w.Add(errorsList[j][i].OuterProduct(weightedInputsList[j][i].Map(Utilities.Sigma)));
                    
                }
                WeightsList[i] = (Matrix)WeightsList[i].Subtract(delta_w.Multiply(StepSize / batchSize));


                //WeightsList[i] = (Matrix)WeightsList[i].MapIndexed((n, m, value) =>
                //{
                //    double sum = 0;
                //    for (int j = 0; j < batchSize; j++)
                //    {
                //        var activation = Utilities.Sigma(weightedInputsList[j][i][m]);
                //        var error = errorsList[j][i][n];
                //        sum += error*activation;
                //    }
                //    return value - StepSize*sum/batchSize;
                //});
            }
        }

        //not needed?
        public void TrainNetwork(DataPoint data)
        {
            List<Vector> activations = GetActivations(data);
            List<Vector> inputs = GetInputs(data);
            List<Vector> errors = GetErrors(inputs, data);

            int biasesCount = BiasesList.Count;
            for (int i = 0; i < biasesCount; i++)
            {
               BiasesList[i].MapIndexed((j, value) => value - StepSize*errors[i][j]);
            }

            int weightsCount = WeightsList.Count;
            for (int i = 0; i < weightsCount; i++)
            {
               WeightsList[i].MapIndexed((m,n,value) => value - StepSize*errors[i][m]*activations[i+1][n]);
            }
        }
        
        public List<Vector> GetActivations(DataPoint data)
        {
            var list = new List<Vector>();
            list.Add(data.Input);
            for (int i = 1; i < WeightsList.Count; i++)
            {
                var x = (Vector)(WeightsList[i - 1].Multiply(list[i - 1]).Add(BiasesList[i - 1])).Map(Utilities.Sigma);
                list.Add(x);
            }
            return list;
        }

        public List<Vector> GetInputs(DataPoint data)
        {
            var list = new List<Vector>();
            var x = (Vector) (WeightsList[0].Multiply(data.Input).Add(BiasesList[0]));
            list.Add(x);
            for (int i = 1; i < WeightsList.Count; i++)
            {
                var a = list[i - 1].Map(Utilities.Sigma);
                var z = (Vector)(WeightsList[0].Multiply(a).Add(BiasesList[0]));
                list.Add(z);
            }
            return list;
        }

        public List<Vector> GetErrors(List<Vector> inputs, DataPoint data)
        {
            var list = new List<Vector>();
            var a = inputs.Last().Map(Utilities.Sigma);
            var grad = (Vector) (a.Subtract(data.Output)).PointwiseMultiply(inputs.Last());
            var x= (Vector)grad.PointwiseMultiply(inputs.Last().Map(Utilities.SigmaDiff));
            list.Add(x);
            for (int i = WeightsList.Count-1; i > 1; i--)
            {
                var d =
                   (Vector)(WeightsList[i + 1].Transpose().Multiply(list[i + 1])).PointwiseMultiply(
                        inputs[i].Map(Utilities.SigmaDiff));
                list.Add(d);
            }
            list.Reverse();
            return list;
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