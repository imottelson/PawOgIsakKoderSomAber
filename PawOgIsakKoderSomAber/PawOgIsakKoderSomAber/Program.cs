using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;

namespace PawOgIsakKoderSomAber
{
    class Program
    {
        static void Main(string[] args)
        {
            NeuralNetwork network = new NeuralNetwork(new List<int> {784, 30, 10}, stepSize: 0.1);

            //TODO: take the paths as inputs
            var trainingData = DataHelper.LoadData("",""); 
            var testData = new List<DataPoint>(); // TODO: Load the test data ???
            
            trainingData.Shuffle();

            int batchSize = 30;
            int epochs = 30;

            //TODO: partition data into batches
            for(int k=0;k<epochs;k++)
            {
                var batch = trainingData.GetRange(k*batchSize, batchSize);
                network.TrainNetwork(batch);
                Console.WriteLine("Output: " + network.Evaulate(trainingData[0].Input)+ "\n\r Target: " + trainingData[0].Output);
                Console.ReadKey();
            }

        }
    }
}
