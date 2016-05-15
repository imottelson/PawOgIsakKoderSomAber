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

        public int NumberOfUpdates;

        public int BatchSize;

        public List<int> SizezList; 

        public List<Vector> BiasesList;

        public List<Matrix> WeightsList; 

        public List<DataPoint> TrainingDataList;

        public List<DataPoint> TestDataList;

        public INeuralNetworkUtil Utilities { private get; set; }

        public Vector Evaulate(Vector input)
        {
            return Utilities.Evaluate(input, WeightsList, BiasesList);
        }

        public void Update(List<DataPoint> batch)
        {
            throw new NotImplementedException();
        }



        public NeuralNetwork()
        {
            this.Utilities = new StandardUtilities();
            this.StepSize = 0.5;
            this.NumberOfUpdates = 50;
            this.BatchSize = 30;
        }

        public NeuralNetwork(List<int> sizes) : this()
        {
            //Sets the weights and biases. Not the data
            throw new NotImplementedException();
        }

    }
}