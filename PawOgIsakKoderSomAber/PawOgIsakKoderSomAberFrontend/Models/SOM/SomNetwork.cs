using System;
using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using PawOgIsakKoderSomAberFrontend.Structs;

namespace PawOgIsakKoderSomAberFrontend.Models.SOM
{
    public class SomNetwork
    {
        public List<SomNode> Weights;

        public int Time;

        public double T0;

        public double L0;

        public int Height;

        public int Width;

        public double Radius;

        public double LearningRate;

        public SomNetwork(int height, int width, double t0, double learningRate, double l0)
        {
            Height = height;
            Width = width;
            T0 = t0;
            L0 = l0;
            Radius = Math.Sqrt((Height * Height + Width * Width) / 4);
            LearningRate = learningRate;
        }

        public SomNetwork(int height, int width, double t0, double learningRate, double l0, int inputDimension):this(height,width,t0,learningRate,l0)
        {
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
        }

        public SomNetwork(int height, int width, double t0, double learningRate, double l0,
            List<DataPoint> souces) : this(height, width, t0, learningRate, l0)
        {
            var weights = new List<SomNode>();
            for (double i = 0; i < Height; i++)
            {
                for (double j = 0; j < Width; j++)
                {
                    int k =Convert.ToInt32(i*width + j);
                    weights.Add(new SomNode()
                    {
                        Position = new DenseVector(new double[] { i, j }),
                        Weight = souces[k].Input
                    });
                }
            }
            Weights = weights;
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
