using Microsoft.AspNetCore.Mvc;
using SF_DSS.Data.Entities;
using SF_DSS.Models;
using SF_DSS.Models.Services;
using System.Collections.Generic;

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
        public async Task<IActionResult> GetResponse(string message, int? convoId)
        {
            var response = await _chatbotService.GetResponse(message, convoId);
            return Ok(new Dictionary<string, object> { { "response", response.Messages.Last().Content }, { "id", response.ID } });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConversations()
        {
            var conversations = await _chatbotService.GetAllConversationsAsync();
            return Ok(conversations);
        }
    }
}