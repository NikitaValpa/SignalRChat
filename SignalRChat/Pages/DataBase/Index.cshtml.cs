using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SignalRChat.Data;
using SignalRChat.Models;

namespace SignalRChat
{
    [Authorize (Roles ="admin")]
    public class MessagesModel : PageModel
    {
        private readonly SignalRChatContext _context;

        public MessagesModel(SignalRChatContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            
        }
    }
}
