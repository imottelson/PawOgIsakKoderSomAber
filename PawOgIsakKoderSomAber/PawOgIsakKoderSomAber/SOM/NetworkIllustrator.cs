using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics;
using MathNet.Numerics.Statistics;

namespace PawOgIsakKoderSomAber
{
    public class NetworkIllustrator
    {
        public Bitmap Illustrate(SomNetwork network)
        {
            //var lengths = (from somNode1 in network.Weights from somNode2 in network.Weights where network.Weights.IndexOf(somNode2) < network.Weights.IndexOf(somNode1) select Distance.Euclidean(somNode2.Weight, somNode1.Weight)).ToList();

            var lengths =
                network.Weights.Select(
                    parentNode => network.Neighbors(parentNode,1.5).Average(childNode => Distance.Euclidean(childNode.Weight, parentNode.Weight))).ToList();

            //var lengths = network.Weights.Select(node => node.Weight.L2Norm()).ToList();
            var mean = lengths.Average();
            var quan = lengths.Quantile(0.02);
            
            var standardDeviation = lengths.StandardDeviation();
            var min = lengths.Min();

            var bitmap = new Bitmap(network.Width,network.Height);

            Graphics g =Graphics.FromImage(bitmap);
            foreach (var node in network.Weights)
            {
                int i = Convert.ToInt32(node.Position[0]);
                int j = Convert.ToInt32(node.Position[1]);
               
                var neighbors = network.Neighbors(node, 1.5);
                var dist = 35 * (neighbors.Average(n => Distance.Euclidean(n.Weight, node.Weight))-quan)/standardDeviation;
                    
                var strength = Convert.ToInt32(dist);
                if (strength > 255)
                {
                    strength = 255;
                }
                if (strength < 0)
                {
                    strength = 0;
                }
                var color = Color.FromArgb(strength,Color.Black);
                bitmap.SetPixel(j,i,color);

            }

            return bitmap;
        }
        
    }
}
