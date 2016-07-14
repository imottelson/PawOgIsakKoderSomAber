using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using Newtonsoft.Json;

namespace PawOgIsakKoderSomAber
{
    public class SomNetwork
    {
        public List<SomNode> Weights;

        public int Time;

        public double T0;

        public int Height;

        public int Width;

        public double Radius;

        public double LearningRate;

        public SomNetwork(int height, int width, int inputDimension, double t0, double learningRate)
        {
            Height = height;
            Width = width;
            T0 = t0;
            var weights = new List<SomNode>();
            for (double i = 0; i < Height; i++)
            {
                for (double j = 0; j < Width; j++)
                {
                    weights.Add(new SomNode()
                    {
                        Position = new DenseVector(new double[] {i, j}),
                        Weight = (Vector) Vector.Build.Random(inputDimension)
                    });
                }
            }
            Weights = weights;
            Radius = Math.Sqrt((Height*Height + Width*Width)/4);
            LearningRate = learningRate;
        }
        public List<SomNode> Neighbors(SomNode parentNode, double delta)
        {
            var list = new List<SomNode>();
            foreach (var node in Weights)
            {
                if (Distance.Euclidean(parentNode.Position, node.Position) < delta)
                {
                    list.Add(node);
                }
            }
            return list;
        }

    }
}
