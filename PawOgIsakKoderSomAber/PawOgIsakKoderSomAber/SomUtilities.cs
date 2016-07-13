using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawOgIsakKoderSomAber
{
    public static class SomUtilities
    {
        public static double Shrink(double t, double decay)
        {
            return Math.Exp(-t/decay);
        }
    }
}
