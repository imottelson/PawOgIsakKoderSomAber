using System.Web.Mvc;
using PawOgIsakKoderSomAberFrontend.Models;
using PawOgIsakKoderSomAberFrontend.Models.NN;

namespace PawOgIsakKoderSomAberFrontend.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BeginRequest()
        {
            //genereate a new request and return an editor view
            var request = new PawOgIsakRequest();
            var trainingData = DataHelper.LoadData("../../../MNIST/train-images.idx3-ubyte", "../../../MNIST/train-labels.idx1-ubyte", numberOfImages: 50000);

            var testData = DataHelper.LoadData("../../../MNIST/t10k-images.idx3-ubyte", "../../../MNIST/t10k-labels.idx1-ubyte", numberOfImages: 10000);
            request.TrainData = trainingData;
            request.TestData = testData;

            request.Network = new NeuralNetwork();

            return View(request);
        }

        public ActionResult Submit(PawOgIsakRequest request)
        {
            return RedirectToAction("CreateNetwork", "NeuralNetwork", new {request = request});
        }
    }
}