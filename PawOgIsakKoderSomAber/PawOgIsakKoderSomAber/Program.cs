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
            NeuralNetwork network = new NeuralNetwork(new List<int> {784, 30, 10});

            var trainingData = new List<DataPoint>(); // TODO: Load the data ???
            var testgData = new List<DataPoint>(); // TODO: Load the data ???
            
            network.TrainingDataList = trainingData;
            network.TestDataList = testgData;
            

            for(int k=0;k<network.NumberOfUpdates;k++)
            {
                var test = new List<DataPoint>();
                network.Update((List<DataPoint>)MathNet.Numerics.Combinatorics.SelectCombination(network.TrainingDataList,network.BatchSize));
            }

        }
    }
}
