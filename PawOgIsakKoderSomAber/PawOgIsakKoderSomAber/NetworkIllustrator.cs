using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics;

namespace PawOgIsakKoderSomAber
{
    public class NetworkIllustrator
    {
        public void Illustrate(SomNetwork network)
        {
            var bitmap = new Bitmap(network.Width,network.Height);

            Graphics g =Graphics.FromImage(bitmap);
            foreach (var node in network.Weights)
            {
                int i = Convert.ToInt32(node.Position[1]);
                int j = Convert.ToInt32(node.Position[0]);
               
                var neighbors = network.Neighbors(node, 1);
                var dist = neighbors.Average(n => Distance.Euclidean(n.Weight, node.Weight));
                    
                var strength = 5* Convert.ToInt32(dist);
                if (strength > 255)
                {
                    strength = 255;
                }
                var color = Color.FromArgb(strength,Color.Black);
                bitmap.SetPixel(j,i,color);
            }
            bitmap.Save("Network.bmp");
        }
        
    }
}
