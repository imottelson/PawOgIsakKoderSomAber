using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace PawOgIsakKoderSomAber
{
    struct DataPoint
    {
        public Vector Input;
        public Vector Output;

        public DataPoint(object o)
        {
            Input = new DenseVector(new double[] {1.0,1.0});
            Output = new DenseVector(new double[] {1.0});
        }
    }
}
