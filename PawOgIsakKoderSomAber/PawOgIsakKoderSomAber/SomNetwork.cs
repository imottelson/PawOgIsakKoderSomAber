using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using Newtonsoft.Json;

namespace PawOgIsakKoderSomAber
{
    public class SomNetwork
    {
        public Vector[,] Weights;

        public int Time;

        public double Decay;

        public int Height;

        public int Width;

        public double Radius2;

        public double LearningRate;

        public SomNetwork(int height, int width, int inputDimension, double decay, double learningRate)
        {
            Height = height;
            Width = width;
            Decay = decay;
            var weights = new Vector[Height,Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    weights[i, j] = (Vector) Vector.Build.Random(inputDimension);
                }
            }
            Weights = weights;
            Radius2 = (Height*Height + Width*Width)/4;
            LearningRate = learningRate;
        }

    }
}
