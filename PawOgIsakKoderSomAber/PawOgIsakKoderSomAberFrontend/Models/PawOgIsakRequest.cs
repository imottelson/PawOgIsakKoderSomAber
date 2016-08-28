using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PawOgIsakKoderSomAberFrontend.Helpers;
using PawOgIsakKoderSomAberFrontend.Models.NN;
using PawOgIsakKoderSomAberFrontend.Structs;
using PawOgIsakKoderSomAberFrontend.Utilities;

namespace PawOgIsakKoderSomAberFrontend.Models
{
    public class PawOgIsakRequest
    {
        public NeuralNetwork Network { get; set; }

        public int BatchSize { get; set; }

        public int Epochs { get; set; }

        public NetworkUtilities networkUtilities = new NetworkUtilities();

        public NetworkHelper networkHelper
        {
            get
            {
              return  new NetworkHelper(networkUtilities);
            } 
        }

        public List<DataPoint> TrainData { get; set; }

        public List<DataPoint> TestData { get; set; } 

    }
}