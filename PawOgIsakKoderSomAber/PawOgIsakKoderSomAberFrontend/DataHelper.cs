using System;
using System.Collections.Generic;
using System.IO;
using PawOgIsakKoderSomAberFrontend.Structs;
using DenseVector = MathNet.Numerics.LinearAlgebra.Double.DenseVector;

namespace PawOgIsakKoderSomAberFrontend
{
    public static class DataHelper
    {
        public static List<DataPoint> LoadData(string imagePath, string labelPath, int numberOfImages)
        {
            var data = new List<DataPoint>();

            
            FileStream imageStream = new FileStream(@imagePath, FileMode.Open);
            FileStream labelStream = new FileStream(@labelPath, FileMode.Open);

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


            double[][] dataArray = new double[numberOfImages][];
            double[][] labelArray=new double[numberOfImages][];

            for (int j = 0; j < numberOfImages; j++)
            {
                labelArray[j] = new double[10];
                labelArray[j][labelReader.ReadByte()] = 1;
            }



            for (int j = 0; j < numberOfImages; j++)
            {
                dataArray[j] = new double[pixelsPrImage];
                for (int i = 0; i < pixelsPrImage; i++)
                {
                    byte b = imageReader.ReadByte();
                    dataArray[j][i] = Convert.ToDouble(b)/255;
                }
            }
                
            for(int j=0;j< numberOfImages; j++)
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
