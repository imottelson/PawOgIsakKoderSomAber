using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;

namespace PawOgIsakKoderSomAber
{
    public static class SomUtilities
    {
        public static double Shrink(double t, double decay)
        {
            return Math.Exp(-t/decay);
        }

        public static double Gaussian(double t, double stdDev)
        {
            return Math.Exp(-t*t/(stdDev*stdDev));
        }

    }
}
