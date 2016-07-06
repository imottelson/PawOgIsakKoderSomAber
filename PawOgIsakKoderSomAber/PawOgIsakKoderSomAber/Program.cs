using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;

namespace PawOgIsakKoderSomAber
{
    class Program
    {
        static void Main(string[] args)
        {
            NeuralNetwork network = new NeuralNetwork(new List<int> {784, 30, 10}, stepSize:0.5,regularizationParameter:0.0001);

            //TODO: take the paths as inputs
            var trainingData = DataHelper.LoadData("../../../MNIST/train-images.idx3-ubyte", "../../../MNIST/train-labels.idx1-ubyte",numberOfImages: 50000); 
            var testData = DataHelper.LoadData("../../../MNIST/t10k-images.idx3-ubyte", "../../../MNIST/t10k-labels.idx1-ubyte",numberOfImages: 10000);

            NetworkUtilities networkUtilities = new NetworkUtilities();

            NetworkHelper networkHelper = new NetworkHelper(networkUtilities);


            int batchSize = 30;
            int epochs = 30;
            int length = trainingData.Count;
            int testLength = testData.Count;

            //TODO: partition data into batches
            for (int k=0;k<epochs;k++)
            {
                
                trainingData.Shuffle();
                for (int j = 0; j < length/batchSize; j++)
                {
                    var batch = trainingData.GetRange(j * batchSize, batchSize);
                    networkHelper.TrainNetwork(network,batch);
                }
                Console.WriteLine("Epoch: "+(k+1));
                Console.WriteLine(testData.Sum(x => x.Output[networkHelper.Evaluate(x.Input,network).AbsoluteMaximumIndex()]) +" / " + testLength);
                
            }
            Console.ReadKey();

        }
    }
}
