using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void UpdateNetwork(SomNetwork network, Bmu bmu, Vector input)
        {
            var radius2 = network.Radius2*SomUtilities.Shrink(network.Time, network.Decay);
            for (int i = 0; i < network.Height; i++)
            {
                for (int j = 0; j < network.Width; j++)
                {
                    if ((i-bmu.X)*(i-bmu.X) + (j-bmu.Y)*(j-bmu.Y) < radius2)
                    {
                        Vector delta = (Vector) (
                            SomUtilities.Shrink(network.Time + Math.Sqrt((i - bmu.X) * (i - bmu.X) + (j - bmu.Y) * (j - bmu.Y)), network.Decay)*
                            network.LearningRate * ( input- network.Weights[i, j]));
                        network.Weights[i, j] = (Vector)(network.Weights[i, j] + delta);
                    }
                }
            }
        }

        public Bmu BestMatchingUnit(SomNetwork network, Vector input)
        {
            //Inelegant.
            var bmu = new Bmu(0, 0, int.MaxValue);
            for (int i = 0; i < network.Height; i++)
            {
                for (int j = 0; j < network.Width; j++)
                {
                    if ((network.Weights[i, j] - input).L2Norm() < bmu.Distance)
                    {
                        bmu.X = i;
                        bmu.Y = j;
                        bmu.Distance = (network.Weights[i, j] - input).L2Norm();
                    }
                }
            }
            return bmu;
        }
    }
}
