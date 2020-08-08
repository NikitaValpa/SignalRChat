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

namespace SignalRChat.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly SignalRChatContext _context; // Экземпляр службы "SignalRChatContext" реализует обращение к таблице "Messages", через класс "SignalRChatContext"

        public IndexModel(ILogger<IndexModel> logger, SignalRChatContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
      
        }

    }
}
