using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UbiChipher.Data;
using UbiChipher.Services;
using UbiChipher.WebDemo.Models;
using UbiChipher.WebDemo.ViewModels;

namespace UbiChipher.WebDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            if (DummyBackend.Verified)
            {
                return RedirectToAction("Profile");
            }

            var model = new RequestViewModel();

            //var request = new Request() { ClaimRequests = new List<string>() { "Name" }, PostBackUri = "ubichipher.com/verify"};
            var request = new Request() { ClaimRequests = new List<string>() { "Name" }, PostBackUri = "http://localhost:51845/api/verify/claims" };

            var requestString = JsonSerializer.Serialize(request);

            RequestGenerationService requetGenerationService = new RequestGenerationService();
            var imageData = await requetGenerationService.CreateQRAsync(requestString);

            model.QRImage = imageData;

            return View(model);
        }

        public IActionResult Profile()
        {
            ViewBag.Message = DummyBackend.Message;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
