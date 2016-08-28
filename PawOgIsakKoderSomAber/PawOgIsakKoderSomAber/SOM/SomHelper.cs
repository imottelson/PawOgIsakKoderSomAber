using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using Newtonsoft.Json;

namespace PawOgIsakKoderSomAber
{
    public class SomHelper
    {
        public void TrainNetwork(SomNetwork network, Vector input)
        {
            var bmu = BestMatchingUnit(network, input);
            UpdateNetwork(network,bmu, input);
            network.Time++;

        }

        public void UpdateNetwork(SomNetwork network, Bmu bmu, Vector input)
        {
            var radius = network.Radius*SomUtilities.Shrink(network.Time, network.T0);
            foreach (SomNode node in network.Weights)
            {
                if (Distance.Euclidean(bmu.Position,node.Position)<radius)
                {
                    Vector delta = (Vector) (
                        SomUtilities.Shrink(network.Time,network.T0)*
                        SomUtilities.Shrink(Distance.Euclidean(bmu.Position, node.Position), network.L0)*
                        network.LearningRate * ( input- node.Weight));
                    node.Weight = (Vector)(node.Weight + delta);
                }
            }
        }

        public Bmu BestMatchingUnit(SomNetwork network, Vector input)
        {
            //Inelegant.
            var bmu = new Bmu(){Distance = int.MaxValue};

            foreach (SomNode node in network.Weights)
            {
                if (Distance.Euclidean(node.Weight , input) < bmu.Distance)
                {
                    bmu.Position = node.Position;
                    bmu.Distance = Distance.Euclidean(node.Weight, input);
                }
            }
            return bmu;
        }

        public Bmu DrawInput(SomNetwork network, DataPoint data)
        {
            var bmu = BestMatchingUnit(network, data.Input);
            var color = new Color();
            switch (data.Output.AbsoluteMaximumIndex())
            {
                case 0:
                    color = Color.Blue;
                    break;
                case 1: 
                    color = Color.BlueViolet;
                    break;
                case 2:
                    color=Color.Turquoise;
                    break;
                case 3:
                    color = Color.Green;
                    break;
                case 4:
                    color = Color.DeepSkyBlue;
                    break;
                case 5:
                    color = Color.Yellow;
                    break;
                case 6:
                    color = Color.Orange;
                    break;
                case 7:
                    color = Color.DeepPink;
                    break;
                case 8:
                    color = Color.Red;
                    break;
                case 9:
                    color = Color.SaddleBrown;
                    break;
                default:
                    color = Color.White;
                    break;
            }
            bmu.Color = color;
            return bmu;
        }
    }
}
