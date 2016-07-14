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
                        SomUtilities.Shrink(network.Time + Distance.Euclidean(bmu.Position, node.Position), network.T0)*
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
    }
}
