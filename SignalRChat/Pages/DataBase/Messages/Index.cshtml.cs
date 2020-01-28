using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Data;

namespace SignalRChat.Pages.DataBase.Messages
{
    public class MessageModel : PageModel
    {
        private readonly SignalRChatContext _context;

        public MessageModel(SignalRChatContext context)
        {
            _context = context;
        }

        public List<Models.Messages> MessagesList { get; set; }

        public async Task OnGetAsync()
        {
            MessagesList = await _context.Messages.ToListAsync();
        }
    }
}