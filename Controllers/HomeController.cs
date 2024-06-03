using Microsoft.AspNetCore.Mvc;
using SF_DSS.Models;
using SF_DSS.Models.Services;
using System.Diagnostics;

namespace SF_DSS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IChatbotService _chatbotService;

        public HomeController(ILogger<HomeController> logger, IChatbotService chatbotService)
        {
            _logger = logger;
            _chatbotService = chatbotService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Chatbot()
        {
            var model = new ChatbotModel
            {
                Conversations = await _chatbotService.GetAllConversationsAsync()
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
