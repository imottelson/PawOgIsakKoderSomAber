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
            NeuralNetwork network = new NeuralNetwork(new List<int> {784, 30, 10}, stepSize:3);

            //TODO: take the paths as inputs
            var trainingData = DataHelper.LoadData("",""); 
            var testData = new List<DataPoint>(); // TODO: Load the test data ???
            
            

            int batchSize = 30;
            int epochs = 30;
            int length = trainingData.Count;

            //TODO: partition data into batches
            for (int k=0;k<epochs;k++)
            {
                
                trainingData.Shuffle();
                for (int j = 0; j < length/batchSize; j++)
                {
                    var batch = trainingData.GetRange(j * batchSize, batchSize);
                    network.TrainNetwork(batch);
                }
                //Console.WriteLine("Output: " + network.Evaulate(trainingData[0].Input)+ "\n\r Target: " + trainingData[0].Output+"\n\r Cost: " + network.Utilities.Cost(trainingData,network.WeightsList,network.BiasesList));
                Console.WriteLine(trainingData.Sum(x => x.Output[network.Evaulate(x.Input)]));
                //Console.ReadKey();
            }
            Console.ReadKey();

        }
    }
}
