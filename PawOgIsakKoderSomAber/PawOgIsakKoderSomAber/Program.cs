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
            NeuralNetwork network = new NeuralNetwork(new List<int> {2, 3, 1});

            var trainingData = new List<DataPoint> {new DataPoint("En abe")}; // TODO: Load the data ???
            var testData = new List<DataPoint>(); // TODO: Load the data ???
            
            for(int k=0;k<50;k++)
            { 
                network.TrainNetwork(trainingData);
            }

        }
    }
}
