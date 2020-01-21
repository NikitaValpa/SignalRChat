using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IList<Models.Messages> Messages { get; set; } // Создаём автосвойство в классе, которое используется на страничке и в методах бэкенда этой странички, это свойство является списком, элементами которого являются объекты модели "Messages"
                                                             // Другими словами, это список объектов "Messages"
        
        public async Task OnGetAsync()
        {
           Messages = await _context.Messages.ToListAsync(); // Присваеваем нашему свойству контекст таблицы Messages нашей базы данных
        }

    }
}
