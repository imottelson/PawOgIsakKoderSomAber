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
        public static List<DataPoint> LoadData(string imagePath, string labelPath)
        {
            var data = new List<DataPoint>();

            
            FileStream imageStream = new FileStream(@"../../../MNIST/train-images.idx3-ubyte", FileMode.Open);
            FileStream labelStream = new FileStream(@"../../../MNIST/train-labels.idx1-ubyte", FileMode.Open);

            BinaryReader imageReader = new BinaryReader(imageStream);
            BinaryReader labelReader = new BinaryReader(labelStream);
            


            //no idea what these things mean
            int magic1 = imageReader.ReadInt32();
            int numImages1 = imageReader.ReadInt32();
            int numRows1 = imageReader.ReadInt32();
            int numCols1 = imageReader.ReadInt32();

            int magic2 = labelReader.ReadInt32();
            int numLabels = labelReader.ReadInt32();

            int pixelsPrImage = 784;
            int numImages = 10000;


            double[][] dataArray = new double[numImages][];
            double[][] labelArray=new double[numImages][];

            for (int j = 0; j < numImages ; j++)
            {
                labelArray[j] = new double[10];
                labelArray[j][labelReader.ReadByte()] = 1;
            }



            for (int j = 0; j < numImages; j++)
            {
                dataArray[j] = new double[pixelsPrImage];
                for (int i = 0; i < pixelsPrImage; i++)
                {
                    byte b = imageReader.ReadByte();
                    dataArray[j][i] = Convert.ToDouble(b)/255;
                }
            }
                
            for(int j=0;j<numImages;j++)
                data.Add(new DataPoint()
                {
                    Input = new DenseVector(dataArray[j]),
                    Output = new DenseVector(labelArray[j])
                });
            imageStream.Close();
            imageReader.Close();
            labelStream.Close();
            labelReader.Close();





            return data;
        }  
    }
}
