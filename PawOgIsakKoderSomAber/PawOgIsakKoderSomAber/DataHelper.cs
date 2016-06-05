using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Complex;
using DenseVector = MathNet.Numerics.LinearAlgebra.Double.DenseVector;

namespace PawOgIsakKoderSomAber
{
    public static class DataHelper
    {
        public static List<DataPoint> LoadData()
        {
            var data = new List<DataPoint>();

            

            FileStream imageStream = new FileStream(@"../../../MNIST/train-images.idx3-ubyte",FileMode.Open);
            FileStream labelStream = new FileStream(@"../../../MNIST/train-labels.idx1-ubyte", FileMode.Open);

            BinaryReader imgReader = new BinaryReader(imageStream);
            BinaryReader labelReader =new BinaryReader(labelStream);


            //no idea what these things mean
            int magic1 = imgReader.ReadInt32();
            int numImages1 = imgReader.ReadInt32();
            int numRows1 = imgReader.ReadInt32();
            int numCols1 = imgReader.ReadInt32();

            int magic2 = labelReader.ReadInt32();
            int numLabels = labelReader.ReadInt32();

            int pixelsPrImage = 784;
            int numImages = 10000;


            double[][] dataArray = new double[numImages][];
            double[][] labelArray=new double[numImages][];

            for (int j = 0; j < numImages ; j++)
            {
                labelArray[j] = new double[10];
                labelArray[j][labelStream.ReadByte()] = 1;
            }
                
                

            for(int j=0; j<numImages;j++)
                for (int i = 0; i < pixelsPrImage; i++)
                {
                    dataArray[j] = new double[pixelsPrImage];
                    dataArray[j][i] = imgReader.ReadByte();
                }
            for(int j=0;j<numImages;j++)
                data.Add(new DataPoint()
                {
                    Input = new DenseVector(dataArray[j]),
                    Output = new DenseVector(labelArray[j])
                });
            
            return data;
        }  
    }
}
