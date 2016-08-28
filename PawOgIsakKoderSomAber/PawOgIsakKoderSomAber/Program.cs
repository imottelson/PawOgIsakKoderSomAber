using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;

namespace PawOgIsakKoderSomAber
{
    class Program
    {
        static void Main(string[] args)
        {
            //NeuralNetwork network = new NeuralNetwork(new List<int> {784, 30, 10}, stepSize:0.5,regularizationParameter:0.0001);

            ////TODO: take the paths as inputs
            var trainingData = DataHelper.LoadData("../../../MNIST/train-images.idx3-ubyte", "../../../MNIST/train-labels.idx1-ubyte", numberOfImages: 50000);

            var testData = DataHelper.LoadData("../../../MNIST/t10k-images.idx3-ubyte", "../../../MNIST/t10k-labels.idx1-ubyte", numberOfImages: 10000);

            NetworkUtilities networkUtilities = new NetworkUtilities();

            NetworkHelper networkHelper = new NetworkHelper(networkUtilities);


            //int batchSize = 50;
            //int epochs = 30;
            //int length = trainingData.Count;
            //int testLength = testData.Count;

            //var network = new NeuralNetwork(new List<int> {784, 30, 10}, 0.1, 0.001);

            //for (int k = 0; k < epochs; k++)
            //{
            //    trainingData.Shuffle();
            //    for (int j = 0; j < length / batchSize; j++)
            //    {
            //        var batch = trainingData.GetRange(j * batchSize, batchSize);
            //        networkHelper.TrainNetwork(network, batch);
            //    }
            //    Console.WriteLine("Epoch: " + (k + 1));
            //    Console.WriteLine(testData.Sum(x => x.Output[networkHelper.Evaluate(x.Input, network).AbsoluteMaximumIndex()]) + " / " + testLength);
            //    DatabaseHelper.SaveNetworkToDatabase(network, k + 1);


            //}
            //Console.ReadKey();
            //trainingData.Shuffle();
            
            SomNetwork somNetwork = new SomNetwork(height: 50, width: 50, t0: 2500,
                learningRate: 0.08, l0:100, inputDimension:784);
            trainingData.Shuffle();
            trainingData = trainingData.Take(10000).ToList();
            SomHelper helper = new SomHelper();
            NetworkIllustrator illustrator = new NetworkIllustrator();

            for (int k = 0; k < trainingData.Count(); k++)
            {
                helper.TrainNetwork(somNetwork, trainingData[k].Input);
            }
            var bitmap = illustrator.Illustrate(somNetwork);

            //Illustrate the first test data points
            for (int k = 0; k < 800; k++)
            {
                var bmu = helper.DrawInput(somNetwork, testData[k]);
                int i = Convert.ToInt32(bmu.Position[0]);
                int j = Convert.ToInt32(bmu.Position[1]);

                bitmap.SetPixel(j,i,bmu.Color);
            }
            bitmap.Save("Network.bmp");
        }
    }
}
