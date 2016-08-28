using System;

namespace PawOgIsakKoderSomAberFrontend.Utilities
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
