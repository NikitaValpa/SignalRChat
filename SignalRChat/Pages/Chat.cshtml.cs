using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Client;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Models;
using SignalRChat.Data;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace SignalRChat.Pages
{
    public class ChatModel : PageModel
    {
        private readonly ILogger<ChatModel> _logger;

        private readonly SignalRChatContext _context; // Экземпляр службы "SignalRChatContext" реализует обращение к таблице "Messages", через класс "SignalRChatContext"

        public ChatModel(ILogger<ChatModel> logger, SignalRChatContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<Messages> MessagesList { get; set; }

        public void OnGet()
        {
            MessagesList = _context.Messages.ToList();
        }

    }
}
