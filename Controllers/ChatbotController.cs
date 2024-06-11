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
        private readonly HttpClient _httpClient;

        public ChatbotController(IChatbotService chatbotService, HttpClient httpClient)
        {
            _chatbotService = chatbotService;
            _httpClient = httpClient;
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
        public async Task<IActionResult> GetJsonResponse(string message, int? convoId)
        {
            var response = await new JSONService(_httpClient).GetResponse(await _chatbotService.GetConversationDetails((int)convoId));
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