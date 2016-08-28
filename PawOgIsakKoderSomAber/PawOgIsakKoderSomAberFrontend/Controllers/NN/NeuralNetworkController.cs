using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PawOgIsakKoderSomAberFrontend.Models.NN;

namespace PawOgIsakKoderSomAberFrontend.Controllers.NN
{
    public class NeuralNetworkController : Controller
    {
        // GET: NeuralNetwork
        public ActionResult CreateNetwork()
        {
            var network = new NeuralNetwork();
            return View(network);
        }

        public ActionResult Submit(NeuralNetwork _network)
        {
            var network = new NeuralNetwork(new List<int> { 784, 30, 10 }, _network.StepSize, _network.RegularizationParameter);
            return TrainNetwork(network);
        }

        public ActionResult TrainNetwork(NeuralNetwork network)
        {
            return View();
        }

    }
}