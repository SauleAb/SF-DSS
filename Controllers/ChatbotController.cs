using Microsoft.AspNetCore.Mvc;
using SF_DSS.Data.Entities;
using SF_DSS.Models;
using SF_DSS.Models.Services;

namespace SF_DSS.Controllers
{
    public class ChatbotController : Controller
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotController(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        public IActionResult Index()
        {
             return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetResponse(string message)
        {
            var response = await  _chatbotService.GetResponse(message);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConversations()
        {
            var conversations = await _chatbotService.GetAllConversationsAsync();
            return Ok(conversations);
        }
    }
}